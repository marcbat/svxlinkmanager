using LanguageExt;
using LanguageExt.Common;

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
    public record StopReflectorCommand(Guid ConfigId, Guid ReflectorId) : IRequest<Validation<Error, Guid>>;

    internal class StopReflectorCommandHandler : IRequestHandler<StopReflectorCommand, Validation<Error, Guid>>
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

        public Task<Validation<Error, Guid>> Handle(StopReflectorCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Arrêt du reflector.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from reflector in config.GetReflector(request.ReflectorId)
                             from _ in DisableReflector(reflector)
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             from ___ in svxlinkService.StopReflector(reflector)
                             select config.Id;

                logger.LogInformation("Le reflector a été arrêté avec succès.");

                return Task.FromResult(result);

        }

        private static Validation<Error, LanguageExt.Unit> DisableReflector(Domain.Entities.Reflector reflector)
        {
            reflector.Enable = false;

            return LanguageExt.Unit.Default;
        }
    }
}
