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
    public record StartEnableReflectors(Guid ConfigId): IRequest<Unit>;

    internal class StartEnableReflectorsHandler : IRequestHandler<StartEnableReflectors, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<StartEnableReflectorsHandler> logger;

        public StartEnableReflectorsHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISvxlinkServiceBase svxlinkService, ILogger<StartEnableReflectorsHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StartEnableReflectors request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Starting default reflector.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from optionReflector in config.GetEnableReflector()
                         select config.Id;

            logger.LogInformation("Starting default reflector ok.");

            return Unit.Value;

        }
    }
}
