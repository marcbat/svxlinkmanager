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
    public record DeleteEcholinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    internal class DeleteEcholinkChannelCommandHandler : IRequestHandler<DeleteEcholinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteEcholinkChannelCommandHandler> logger;

        public DeleteEcholinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteEcholinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Suppression du echolink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.EcholinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le echolink channel.");

                config.DeleteEcholinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Echolink channel supprimé.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du echolink channel.");
                throw new SvxlinkManagerException("Impossible de supprimer le echolink channel.", ex);
            }
        }
    }
}
