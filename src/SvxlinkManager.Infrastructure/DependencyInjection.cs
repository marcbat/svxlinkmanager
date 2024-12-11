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

            services.AddSingleton<SvxlinkServiceBase>();
            services.AddSingleton<ISvxlinkServiceBase>(sp => sp.GetRequiredService<SvxlinkServiceBase>());
            services.AddSingleton<ISvxlinkActionService>(sp => sp.GetRequiredService<SvxlinkServiceBase>());
            services.AddSingleton<ISa818Service, Sa818Service>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IIniService, IniService>();
            services.AddScoped<IAuthentificationService, AuthentificationService>();

            return services;
        }
    }

}