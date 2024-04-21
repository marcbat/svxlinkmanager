using MediatR;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Queries
{
    public record GetAvanceSvxlinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<AdvanceSvxlinkChannel>;

    internal class GetAvanceSvxlinkChannelQueryHandler : IRequestHandler<GetAvanceSvxlinkChannelByIdQuery, AdvanceSvxlinkChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetAvanceSvxlinkChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<AdvanceSvxlinkChannel> Handle(GetAvanceSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.AdvanceSvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid);

                if (channel is null)
                    throw new SvxlinkManagerException("Impossible de trouver le canal avancé Svxlink.");

                return channel;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le canal avancé Svxlink.", ex);
            }
        }
    }
}
