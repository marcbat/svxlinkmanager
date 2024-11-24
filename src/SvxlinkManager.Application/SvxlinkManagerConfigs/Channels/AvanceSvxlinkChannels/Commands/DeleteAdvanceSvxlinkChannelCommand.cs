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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Commands
{
    public record DeleteAdvanceSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, Guid>>;

    internal class DeleteAdvanceSvxlinkChannelCommandHandler : IRequestHandler<DeleteAdvanceSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteAdvanceSvxlinkChannelCommandHandler> logger;

        public DeleteAdvanceSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteAdvanceSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Suppression du canal avancé Svxlink.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from channel in config.GetAdvanceSvxlinkChannel(request.ChannelGuid)
                         from _ in config.DeleteAdvanceSvxlinkChannel(request.ChannelGuid)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Canal avancé Svxlink supprimé.");

            return Task.FromResult(result);

        }
    }
}
