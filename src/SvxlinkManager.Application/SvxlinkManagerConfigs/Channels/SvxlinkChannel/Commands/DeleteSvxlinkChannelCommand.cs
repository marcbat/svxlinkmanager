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
    public record DeleteSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    internal class DeleteSvxlinkChannelCommandHandler : IRequestHandler<DeleteSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteSvxlinkChannelCommandHandler> logger;

        public DeleteSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Suppression du svxlink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.SvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le svxlink channel.");

                config.DeleteSvxlinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Svxlink channel supprimé.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du svxlink channel.");
                throw new SvxlinkManagerException("Impossible de supprimer le svxlink channel.", ex);
            }
        }
    }

}
