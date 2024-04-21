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
    public record ActivateSvxlinkChannelCommand(Guid ConfigId, Guid ChannelId) : IRequest<Unit>;

    internal class ActivateSvxlinkChannelCommandHandler : IRequestHandler<ActivateSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<ActivateSvxlinkChannelCommandHandler> logger;

        public ActivateSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                    ILogger<ActivateSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(ActivateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SvxlinkManagerConfigAggregate config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Svxlink channel activated successfully.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to activate Svxlink channel.");
                throw new SvxlinkManagerException("Failed to activate Svxlink channel.", ex);
            }
        }
    }
}
