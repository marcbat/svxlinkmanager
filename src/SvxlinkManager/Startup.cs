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
using Microsoft.ApplicationInsights.SnapshotCollector;

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
      services.AddServerSideBlazor();
      services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

      services.AddSingleton<Data.IDbContextFactory<ApplicationDbContext>, DbContextFactory<ApplicationDbContext>>();
      services.AddSingleton<IRepositories, Repositories.Repositories>();
      services.AddSingleton<SvxLinkService>();
      services.AddSingleton<ScanService>();

#if DEBUG
      services.AddSingleton<ISa818Service, Sa818ServiceMockup>();
      services.AddSingleton<IWifiService, WifiServiceMockup>();
#endif

#if RELEASE
      services.AddSingleton<ISa818Service, Sa818Service>();
      services.AddSingleton<IWifiService, WifiService>();
#endif

      services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
      //services.AddSingleton<ITelemetryInitializer, SvxlinkManagerTelemetry>();
      services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
      services.AddSnapshotCollector((configuration) => Configuration.Bind(nameof(SnapshotCollectorConfiguration), configuration));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
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

        // set telemetry global settings
        var telemetry = serviceScope.ServiceProvider.GetRequiredService<TelemetryClient>();
        var deviceId = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString(); ;
        telemetry.Context.GlobalProperties["DeviceId"] = deviceId;
        telemetry.Context.Device.Id = deviceId;
        telemetry.Context.Device.OperatingSystem = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        telemetry.Context.Component.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });

      // ajout de l'utilisateur admin
      ApplicationDbInitializer.SeedUsers(userManager);

      // Copy du fichier logic.tcl
      if (!Directory.Exists("/usr/share/svxlink/events.d/local"))
        Directory.CreateDirectory("/usr/share/svxlink/events.d/local");

      File.Copy($"{Directory.GetCurrentDirectory()}/SvxlinkConfig/Logic.tcl", "/usr/share/svxlink/events.d/local/Logic.tcl", true);
    }
  }
}