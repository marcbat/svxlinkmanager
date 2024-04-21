using MediatR;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries
{
    public record GetAllEcholinkChannelsQuery(Guid ConfigId) : IRequest<IEnumerable<EcholinkChannel>>;

    internal class GetAllEcholinkChannelsQueryHandler : IRequestHandler<GetAllEcholinkChannelsQuery, IEnumerable<EcholinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetAllEcholinkChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<IEnumerable<EcholinkChannel>> Handle(GetAllEcholinkChannelsQuery request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            return config.EcholinkChannels.ToList();
        }
    }
}
