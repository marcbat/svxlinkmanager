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
    public record StopReflectorCommand(Guid ConfigId, Guid ReflectorId) : IRequest<Unit>;

    public class StopReflectorCommandHandler : IRequestHandler<StopReflectorCommand, Unit>
    {
        private readonly string applicationPath = Directory.GetCurrentDirectory();
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly IFileService fileService;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<StopReflectorCommandHandler> logger;

        public StopReflectorCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, IFileService fileService, ISvxlinkServiceBase svxlinkService, ILogger<StopReflectorCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.fileService = fileService;
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StopReflectorCommand request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            var reflector = config.Reflectors.FirstOrDefault(x => x.Id == request.ReflectorId);

            if (reflector is null)
            {
                logger.LogError("Impossible d'arrêter le reflector. Le reflector n'existe pas.");
                throw new SvxlinkManagerException("Impossible d'arrêter le reflector. Le reflector n'existe pas.");
            }

            reflector.Enable = false;

            await svxlinkManagerConfigRepository.UpdateAsync(config);

            svxlinkService.StopReflector(reflector);

            logger.LogInformation("Le reflector a été arrêté avec succès.");

            return Unit.Value;
        }
    }
}
