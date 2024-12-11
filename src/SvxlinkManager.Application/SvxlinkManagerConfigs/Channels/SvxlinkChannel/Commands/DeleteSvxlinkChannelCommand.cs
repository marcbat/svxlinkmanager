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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands
{
    public record DeleteSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, Guid>>;

    internal class DeleteSvxlinkChannelCommandHandler : IRequestHandler<DeleteSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteSvxlinkChannelCommandHandler> logger;

        public DeleteSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Suppression du svxlink channel.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                             from channel in config.GetSvxlinkChannel(request.ChannelGuid)
                             from _ in config.DeleteSvxlinkChannel(request.ChannelGuid)
                             select config.Id;

                logger.LogInformation("Svxlink channel supprimé.");

                return Task.FromResult(result);
        }
    }

}
