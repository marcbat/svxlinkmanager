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
    public record GetAllSvxlinChannelQuery(Guid ConfigId): IRequest<List<Domain.Entities.SvxlinkChannel>>;

    public class GetAllSvxlinChannelQueryHandler : IRequestHandler<GetAllSvxlinChannelQuery, List<Domain.Entities.SvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetAllSvxlinChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<List<Domain.Entities.SvxlinkChannel>> Handle(GetAllSvxlinChannelQuery request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            return config.SvxlinkChannels.ToList();
        }
    }
}
