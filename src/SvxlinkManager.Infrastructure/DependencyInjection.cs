using Microsoft.Extensions.DependencyInjection;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Infrastructure.Services;

namespace SvxlinkManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ISoundRepository, SoundRepository>();
            services.AddSingleton<ISvxlinkManagerConfigRepository, SvxlinkManagerConfigRepository>();

            services.AddSingleton<ISvxlinkServiceBase, SvxlinkServiceBase>();

            return services;
        }
    }

}