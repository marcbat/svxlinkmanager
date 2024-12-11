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
        private readonly ChannelService channelService;
        private readonly ILogger<StartDefaultChannelCommandHandler> logger;

        public StartDefaultChannelCommandHandler(ChannelService channelService, ILogger<StartDefaultChannelCommandHandler> logger)
        {
            this.channelService = channelService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(StartDefaultChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Starting default channel.");

                var result = channelService.StartDefaultChannel(request.ConfigId);

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
