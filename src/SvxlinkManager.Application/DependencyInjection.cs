using Microsoft.Extensions.DependencyInjection;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Installer;
using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils;

namespace SvxlinkManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddSingleton<ChannelService>();
            services.AddScoped<InstallerService>();
            services.AddScoped<RadioProfilService>();

            return services;
        }
    }
}