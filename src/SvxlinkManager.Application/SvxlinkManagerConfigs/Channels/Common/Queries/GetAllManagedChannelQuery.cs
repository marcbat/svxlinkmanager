using MediatR;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries
{
    public record GetAllManagedChannelQuery(Guid ConfigId) : IRequest<IEnumerable<ManagedChannel>>;

    public class GetAllManagedChannelQueryHandler : IRequestHandler<GetAllManagedChannelQuery, IEnumerable<ManagedChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetAllManagedChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<IEnumerable<ManagedChannel>> Handle(GetAllManagedChannelQuery request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            return config.GetSvxlinkAndEcholinkChannels();
        }
    }
}
