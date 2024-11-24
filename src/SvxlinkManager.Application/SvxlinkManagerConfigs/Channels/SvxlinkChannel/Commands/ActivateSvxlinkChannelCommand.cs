using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands
{
    public record ActivateSvxlinkChannelCommand(Guid ConfigId, Guid ChannelId) : IRequest<Validation<Error, Guid>>;

    internal class ActivateSvxlinkChannelCommandHandler : IRequestHandler<ActivateSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<ActivateSvxlinkChannelCommandHandler> logger;

        public ActivateSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                    ILogger<ActivateSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error,Guid>> Handle(ActivateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
           
                logger.LogInformation("Activating Svxlink channel.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from _ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;


                logger.LogInformation("Svxlink channel activated successfully.");

                return Task.FromResult(result);
           
        }
    }
}
