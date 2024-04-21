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
    public record DeleteAdvanceSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    internal class DeleteAdvanceSvxlinkChannelCommandHandler : IRequestHandler<DeleteAdvanceSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteAdvanceSvxlinkChannelCommandHandler> logger;

        public DeleteAdvanceSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteAdvanceSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Suppression du canal avancé Svxlink.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.AdvanceSvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le canal avancé Svxlink.");

                config.DeleteAdvanceSvxlinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Canal avancé Svxlink supprimé.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du canal avancé Svxlink.");
                throw new SvxlinkManagerException("Impossible de supprimer le canal avancé Svxlink.", ex);
            }
        }
    }
}
