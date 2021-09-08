using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spotnik.Gui.Areas.Identity;
using SvxlinkManager.Data;
using SvxlinkManager.Repositories;
using SvxlinkManager.Service;
using System.IO;
using SvxlinkManager.ServiceMockup;
using Microsoft.ApplicationInsights.Extensibility;
using SvxlinkManager.Telemetry;
using Microsoft.ApplicationInsights;
using DeviceId;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace SvxlinkManager
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlite(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddRazorPages();
      services.AddControllers();

      services.AddServerSideBlazor();
      services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

      services.AddSingleton<Data.IDbContextFactory<ApplicationDbContext>, DbContextFactory<ApplicationDbContext>>();
      services.AddSingleton<IRepositories, Repositories.Repositories>();
      services.AddSingleton<SvxLinkService>();
      services.AddSingleton<ScanService>();
      services.AddSingleton<UpdaterService>();
      services.AddSingleton<IIniService, IniService>();

#if DEBUG
      services.AddSingleton<ISa818Service, Sa818ServiceMockup>();
      services.AddSingleton<IWifiService, WifiServiceMockup>();
#endif

#if RELEASE
      services.AddSingleton<ISa818Service, Sa818Service>();
      services.AddSingleton<IWifiService, WifiService>();
#endif

      services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
      services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<ITelemetryInitializer, SvxlinkManagerTelemetry>();

      // Password complexity
      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
      });

      services.AddLocalization(options => options.ResourcesPath = "Resources");
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, NavigationManager navigationManager)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        // migrate database
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        // start default channel
        var svxlinkservice = serviceScope.ServiceProvider.GetRequiredService<SvxLinkService>();
        svxlinkservice.StartDefaultChannel();

        // start enable reflector
        svxlinkservice.StartEnableReflector();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      var supportedCultures = new[] { "en-US", "fr-FR" };
      var localizationOptions = new RequestLocalizationOptions()
          .SetDefaultCulture(supportedCultures[0])
          .AddSupportedCultures(supportedCultures)
          .AddSupportedUICultures(supportedCultures);

      app.UseRequestLocalization(localizationOptions);

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });

      // Copy du fichier logic.tcl
      if (!Directory.Exists("/usr/share/svxlink/events.d/local"))
        Directory.CreateDirectory("/usr/share/svxlink/events.d/local");

      File.Copy($"{Directory.GetCurrentDirectory()}/SvxlinkConfig/Logic.tcl", "/usr/share/svxlink/events.d/local/Logic.tcl", true);
      File.Copy($"{Directory.GetCurrentDirectory()}/SvxlinkConfig/Locale.tcl", "/usr/share/svxlink/events.d/local/Locale.tcl", true);
    }
  }
}