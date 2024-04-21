using MediatR;
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

        public DeleteEcholinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<Unit> Handle(DeleteEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.EcholinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le echolink channel.");

                config.DeleteEcholinkChannel(request.ChannelGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de supprimer le echolink channel.", ex);
            }
        }
    }
}
