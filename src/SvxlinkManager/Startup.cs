using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Spotnik.Gui.Areas.Identity;

using SvxlinkManager.Application;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands;
using SvxlinkManager.Infrastructure;
using SvxlinkManager.Infrastructure.Services;
using SvxlinkManager.Service;
using SvxlinkManager.ServiceMockup;

using Serilog;

using System.IO;

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
            services.Configure<SvxlinkManagerOptions>(Configuration.GetSection("SvxlinkManager"));

            services.AddSerilog();

            services.AddSingleton<ILiteDbContext, LiteDbContext>();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                      .AddUserStore<LiteDbUserStore<ApplicationUser>>();

            services.AddApplication();
            services.AddInfrastructure();

            services.AddRazorPages();

            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

            services.AddSingleton<ScanService>();
            services.AddSingleton<UpdaterService>();


#if DEBUG
            services.AddSingleton<ISa818Service, Sa818ServiceMockup>();
            services.AddSingleton<IWifiService, WifiServiceMockup>();
#endif

#if RELEASE
      services.AddSingleton<ISa818Service, Sa818Service>();
      services.AddSingleton<IWifiService, WifiService>();
#endif

            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediatr = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
                var options = serviceScope.ServiceProvider.GetRequiredService<IOptions<SvxlinkManagerOptions>>();


                // Seed default config if not exists
                var result1 = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(options.Value.ConfigId));

                // start default channel
                var result2 = await mediatr.Send(new StartDefaultChannelCommand(options.Value.ConfigId));


                // start enable reflector
                var result3 = await mediatr.Send(new StartEnableReflectors(options.Value.ConfigId));

                var result = from create in result1
                             from start in result2
                             from enable in result3
                             select enable;

                if (result.IsFail)
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                    result.FailToList().ToList().ForEach(e => logger.LogError(e.Message));
                                    }
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

            // Copy du fichier logic.tcl
            if (!Directory.Exists("/usr/share/svxlink/events.d/local"))
                Directory.CreateDirectory("/usr/share/svxlink/events.d/local");

            File.Copy($"{Directory.GetCurrentDirectory()}/SvxlinkConfig/Logic.tcl", "/usr/share/svxlink/events.d/local/Logic.tcl", true);
            File.Copy($"{Directory.GetCurrentDirectory()}/SvxlinkConfig/Locale.tcl", "/usr/share/svxlink/events.d/local/Locale.tcl", true);
        }
    }
}