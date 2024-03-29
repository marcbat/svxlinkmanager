using Microsoft.Extensions.DependencyInjection;

using SvxlinkManager.Application.Interfaces;

namespace SvxlinkManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ISoundRepository, SoundRepository>();
            services.AddSingleton<ISvxlinkManagerConfigRepository, SvxlinkManagerConfigRepository>();

            return services;
        }
    }

}