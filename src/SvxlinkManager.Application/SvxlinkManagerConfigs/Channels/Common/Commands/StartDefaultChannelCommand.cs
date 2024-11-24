using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System.Linq;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record StartDefaultChannelCommand(Guid ConfigId) : IRequest<Validation<Error, Guid>>;

    internal class StartDefaultChannelCommandHandler : IRequestHandler<StartDefaultChannelCommand, Validation<Error, Guid>>
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

        public Task<Validation<Error, Guid>> Handle(StartDefaultChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Starting default channel.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from channel in config.GetDefaultChannel()
                             where channel.IsNone
                             from _ in svxlinkService.StopSvxlink()
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Starting default channel.");

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting default channel.");

                throw new Exception("Error starting default channel.",ex);
            }
        }
    }
}
