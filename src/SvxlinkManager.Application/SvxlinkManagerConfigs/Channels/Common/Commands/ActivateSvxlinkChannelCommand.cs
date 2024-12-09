using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record ActivateSvxlinkChannelCommand(Guid ConfigId, Guid IdSvxlinkChannel) : IRequest<Validation<Error, Guid>>;

    internal class ActivateChannelCommandHandler : IRequestHandler<ActivateSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ChannelService channelService;
        private readonly ILogger<ActivateChannelCommandHandler> logger;

        public ActivateChannelCommandHandler(ChannelService channelService, ILogger<ActivateChannelCommandHandler> logger)
        {
            this.channelService = channelService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(ActivateSvxlinkChannelCommand request, CancellationToken cancellationToken) => 
             Task.FromResult(channelService.ActivateChannel(request.ConfigId, request.IdSvxlinkChannel));
    }
}
