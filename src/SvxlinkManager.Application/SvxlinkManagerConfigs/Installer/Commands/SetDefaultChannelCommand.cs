using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record SetDefaultChannelCommand(Guid ConfigId, Guid ChannelId) : IRequest<Unit>;

    public class SetDefaultChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<SetDefaultChannelCommandHandler> logger) : IRequestHandler<SetDefaultChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ILogger<SetDefaultChannelCommandHandler> logger = logger;

        public async Task<Unit> Handle(SetDefaultChannelCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Début de la configuration du channel par defaut.");

            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            var channel = config.SvxlinkChannels.FirstOrDefault(c => c.Id == request.ChannelId);
            if (channel is null)
            {
                logger.LogWarning("Channel not found.");

                return Unit.Value;
            }

            channel.IsDefault = true;
            channel.IsTemporized = false;

            await svxlinkManagerConfigRepository.UpdateAsync(config);

            logger.LogInformation("Le channel par defaut a été configuré.");

            return Unit.Value;
        }
    }
}
