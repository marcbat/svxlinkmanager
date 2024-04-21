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
    public record GetAllSvxlinChannelQuery(Guid ConfigId): IRequest<IEnumerable<Domain.Entities.SvxlinkChannel>>;

    internal class GetAllSvxlinChannelQueryHandler : IRequestHandler<GetAllSvxlinChannelQuery, IEnumerable<Domain.Entities.SvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllSvxlinChannelQueryHandler> logger;

        public GetAllSvxlinChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllSvxlinChannelQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<Domain.Entities.SvxlinkChannel>> Handle(GetAllSvxlinChannelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les canaux Svxlink.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Récupération de tous les canaux Svxlink réussie.");

                return config.SvxlinkChannels;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Erreur lors de la récupération de tous les canaux Svxlink.");
                throw new Exception("Erreur lors de la récupération de tous les canaux Svxlink.", ex);
            }
        }
    }
}
