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
    public record GetAllAdvanceChannelsQuery(Guid ConfigId) : IRequest<IEnumerable<AdvanceSvxlinkChannel>>;

    internal class GetAllAdvanceChannelsQueryHandler : IRequestHandler<GetAllAdvanceChannelsQuery, IEnumerable<AdvanceSvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetAllAdvanceChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<IEnumerable<AdvanceSvxlinkChannel>> Handle(GetAllAdvanceChannelsQuery request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            return config.AdvanceSvxlinkChannels;
        }
    }
}
