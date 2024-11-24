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
    public record StartReflectorCommand(Guid ConfigId, Guid ReflectorId) : IRequest<Validation<Error, Guid>>;

    internal class StartReflectorCommandHandler : IRequestHandler<StartReflectorCommand, Validation<Error, Guid>>
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

        public Task<Validation<Error, Guid>> Handle(StartReflectorCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Démarrage du reflector.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from reflector in config.GetReflector(request.ReflectorId)
                             from _ in EnableReflector(reflector)
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             from ___ in fileService.WriteReflectorConfig(reflector)
                             from ____ in svxlinkService.StartReflector(reflector, pidFile: $"/var/run/reflector-{reflector.Id}.pid", runAs: "root", configFile: $"{applicationPath}/SvxlinkConfig/svxreflector-{reflector.Id}.conf")
                             select config.Id;

            return Task.FromResult(result);

        }

        private static Validation<Error, LanguageExt.Unit> EnableReflector(Domain.Entities.Reflector reflector)
        {
            reflector.Enable = true;

            return LanguageExt.Unit.Default;
        }
    }
}
