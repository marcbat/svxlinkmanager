using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record DeleteManagedChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    public class DeleteManagedChannelCommandHandler : IRequestHandler<DeleteManagedChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteManagedChannelCommandHandler> logger;

        public DeleteManagedChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteManagedChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteManagedChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                config.DeleteManagedChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible de supprimer le managed channel.");
                throw new SvxlinkManagerException("Impossible de supprimer le managed channel.", ex);
            }
        }
    }
}
