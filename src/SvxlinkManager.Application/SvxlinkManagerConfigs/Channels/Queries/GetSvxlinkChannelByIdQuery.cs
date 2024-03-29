using MediatR;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Queries
{
    public record GetSvxlinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<SvxlinkChannel>;

    public class GetSvxlinkChannelByIdQueryHandler : IRequestHandler<GetSvxlinkChannelByIdQuery, SvxlinkChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetSvxlinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<SvxlinkChannel> Handle(GetSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                return config.SvxlinkChannels.Single(c => c.Id == request.ChannelGuid);
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le svxlink channel.", ex);
            }
        }
    }
}
