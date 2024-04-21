using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands
{
    public record StartReflectorCommand(Guid ConfigId, Guid ReflectorId) : IRequest<Unit>;

    internal class StartReflectorCommandHandler : IRequestHandler<StartReflectorCommand, Unit>
    {
        private readonly string applicationPath = Directory.GetCurrentDirectory();
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly IFileService fileService;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<StartReflectorCommandHandler> logger;

        public StartReflectorCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, IFileService fileService, ISvxlinkServiceBase svxlinkService, ILogger<StartReflectorCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.fileService = fileService;
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StartReflectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Démarrage du reflector.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var reflector = config.Reflectors.FirstOrDefault(x => x.Id == request.ReflectorId);

                if (reflector is null)
                {
                    logger.LogError("Impossible de démarrer le reflector. Le reflector n'existe pas.");
                    throw new SvxlinkManagerException("Impossible de démarrer le reflector. Le reflector n'existe pas.");
                }

                reflector.Enable = true;

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                fileService.WriteReflectorConfig(reflector);

                logger.LogInformation("Le fichier de configuration du reflector a été écrit avec succès.");

                svxlinkService.StartReflector(reflector, pidFile: $"/var/run/reflector-{reflector.Id}.pid", runAs: "root", configFile: $"{applicationPath}/SvxlinkConfig/svxreflector-{reflector.Id}.conf");

                logger.LogInformation("Le reflector a été démarré avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors du démarrage du reflector.");
                throw new Exception("Erreur lors du démarrage du reflector.", ex);
            }
        }
    }
}
