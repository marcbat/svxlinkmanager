using MediatR;

using Microsoft.Extensions.Logging;

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

    internal class GetSvxlinkChannelByIdQueryHandler : IRequestHandler<GetSvxlinkChannelByIdQuery, Domain.Entities.SvxlinkChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetSvxlinkChannelByIdQueryHandler> logger;

        public GetSvxlinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetSvxlinkChannelByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Domain.Entities.SvxlinkChannel> Handle(GetSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération du svxlink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var channel = config.SvxlinkChannels.SingleOrDefault(c => c.Id == request.ChannelGuid);

                if (channel is null)
                    throw new SvxlinkManagerException("Impossible de trouver le svxlink channel.");

                logger.LogInformation("Svxlink channel trouvé.");

                return channel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération du svxlink channel.");
                throw new SvxlinkManagerException("Impossible de récupérer le svxlink channel.", ex);
            }
        }
    }
}
