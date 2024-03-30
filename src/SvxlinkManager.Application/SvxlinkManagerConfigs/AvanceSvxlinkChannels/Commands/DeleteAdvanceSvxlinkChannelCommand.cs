using MediatR;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.AvanceSvxlinkChannels.Commands
{
    public record DeleteAdvanceSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    public class DeleteAdvanceSvxlinkChannelCommandHandler : IRequestHandler<DeleteAdvanceSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public DeleteAdvanceSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<Unit> Handle(DeleteAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.AdvanceSvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le canal avancé Svxlink.");

                config.DeleteAdvanceSvxlinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de supprimer le canal avancé Svxlink.", ex);
            }
        }
    }
}
