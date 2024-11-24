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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands
{
    public record DeleteEcholinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, Guid>>;

    internal class DeleteEcholinkChannelCommandHandler : IRequestHandler<DeleteEcholinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteEcholinkChannelCommandHandler> logger;

        public DeleteEcholinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteEcholinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteEcholinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Suppression du echolink channel.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from channel in config.GetEcholinkChannel(request.ChannelGuid)
                         from _ in config.DeleteEcholinkChannel(request.ChannelGuid)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Echolink channel supprimé.");

            return Task.FromResult(result);

        }
    }
}
