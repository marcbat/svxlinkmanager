using Microsoft.Extensions.DependencyInjection;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Installer;

namespace SvxlinkManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddSingleton<ChannelService>();
            services.AddScoped<InstallerService>();

            return services;
        }
    }
}