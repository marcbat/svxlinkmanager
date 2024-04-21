using MediatR;

using Microsoft.Extensions.Logging;

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
        private readonly ILogger<GetEchoLinkChannelByIdQueryHandler> logger;

        public GetEchoLinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetEchoLinkChannelByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<EcholinkChannel> Handle(GetEchoLinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération du EchoLink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.EcholinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid);

                if (channel is null)
                    throw new SvxlinkManagerException("Impossible de trouver le EchoLink channel.");

                logger.LogInformation("EchoLink channel trouvé.");

                return channel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération du EchoLink channel.");
                throw new SvxlinkManagerException("Impossible de récupérer le EchoLink channel.", ex);
            }
        }
    }
}
