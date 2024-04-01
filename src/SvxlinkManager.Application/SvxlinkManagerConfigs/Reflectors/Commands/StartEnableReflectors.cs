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

    public class StartEnableReflectorsHandler : IRequestHandler<StartEnableReflectors, Unit>
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
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var reflectors = config.Reflectors.Where(r => r.Enable).ToList();

                if (!reflectors.Any())
                {
                    logger.LogWarning("No default reflector found.");

                    return Unit.Value;
                }


                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting default reflector.");

                throw;
            }
        }
    }
}
