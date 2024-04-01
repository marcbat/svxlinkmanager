using MediatR;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries
{
    public record GetSvxlinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Domain.Entities.SvxlinkChannel>;

    public class GetSvxlinkChannelByIdQueryHandler : IRequestHandler<GetSvxlinkChannelByIdQuery, Domain.Entities.SvxlinkChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetSvxlinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<Domain.Entities.SvxlinkChannel> Handle(GetSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.SvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid);

                if (channel is null)
                    throw new SvxlinkManagerException("Impossible de trouver le svxlink channel.");

                return channel;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le svxlink channel.", ex);
            }
        }
    }
}
