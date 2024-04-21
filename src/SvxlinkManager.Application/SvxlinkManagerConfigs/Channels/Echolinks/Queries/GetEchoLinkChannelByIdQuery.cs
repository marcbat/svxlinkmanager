using MediatR;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries
{
    public record GetEchoLinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<EcholinkChannel>;

    internal class GetEchoLinkChannelByIdQueryHandler : IRequestHandler<GetEchoLinkChannelByIdQuery, EcholinkChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetEchoLinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<EcholinkChannel> Handle(GetEchoLinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.EcholinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid);

                if (channel is null)
                    throw new SvxlinkManagerException("Impossible de trouver le EchoLink channel.");

                return channel;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le EchoLink channel.", ex);
            }
        }
    }
}
