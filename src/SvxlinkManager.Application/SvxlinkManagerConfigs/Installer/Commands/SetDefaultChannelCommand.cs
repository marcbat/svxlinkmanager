using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record SetDefaultChannelCommand(Guid ConfigId, Guid ChannelId) : IRequest<Validation<Error, Guid>>;

    internal class SetDefaultChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<SetDefaultChannelCommandHandler> logger) : IRequestHandler<SetDefaultChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ILogger<SetDefaultChannelCommandHandler> logger = logger;

        public Task<Validation<Error, Guid>> Handle(SetDefaultChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Début de la configuration du channel par defaut.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from channel in config.GetSvxlinkChannel(request.ChannelId)
                         from _ in SetAsDefault(channel)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Le channel par defaut a été configuré.");

            return Task.FromResult(result);

        }

        private static Validation<Error, LanguageExt.Unit> SetAsDefault(SvxlinkChannel channel)
        {
            channel.IsDefault = true;
            channel.IsTemporized = false;

            return LanguageExt.Unit.Default;
        }
    }
}
