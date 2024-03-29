using MediatR;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Commands
{
    public record DeleteSvxlinkChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Unit>;

    public class DeleteSvxlinkChannelCommandHandler : IRequestHandler<DeleteSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public DeleteSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<Unit> Handle(DeleteSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.SvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le svxlink channel.");
                
                config.DeleteSvxlinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de supprimer le svxlink channel.", ex);
            }
        }
    }

}
