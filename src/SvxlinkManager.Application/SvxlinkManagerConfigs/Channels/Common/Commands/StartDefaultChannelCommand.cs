using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record StartDefaultChannelCommand(Guid ConfigId) : IRequest<Unit>;

    internal class StartDefaultChannelCommandHandler : IRequestHandler<StartDefaultChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<StartDefaultChannelCommandHandler> logger;

        public StartDefaultChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISvxlinkServiceBase svxlinkService, ILogger<StartDefaultChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StartDefaultChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Starting default channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var channel = config.GetManagedChannels().Where(c => c.IsDefault).SingleOrDefault();

                if (channel is null)
                {
                    logger.LogWarning("No default channel found.");

                    svxlinkService.StopSvxlink();

                    return Unit.Value;
                }

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Starting default channel.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting default channel.");

                throw new Exception("Error starting default channel.",ex);
            }
        }
    }
}
