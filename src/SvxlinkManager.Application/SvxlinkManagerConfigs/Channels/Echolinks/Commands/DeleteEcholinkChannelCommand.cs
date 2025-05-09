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
        private readonly ChannelService echolinkService;
        private readonly ILogger<DeleteEcholinkChannelCommandHandler> logger;

        public DeleteEcholinkChannelCommandHandler(ChannelService channelService, ILogger<DeleteEcholinkChannelCommandHandler> logger)
        {
            this.echolinkService = channelService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteEcholinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Suppression du echolink channel.");

            var result = echolinkService.DeleteEcholink(request.ConfigGuid, request.ChannelGuid);

            logger.LogInformation("Echolink channel supprimé.");

            return Task.FromResult(result);

        }
    }
}
