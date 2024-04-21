using MediatR;

using Microsoft.Extensions.Logging;

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
        private readonly ILogger<GetAllAdvanceChannelsQueryHandler> logger;

        public GetAllAdvanceChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllAdvanceChannelsQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<AdvanceSvxlinkChannel>> Handle(GetAllAdvanceChannelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les canaux avancés Svxlink.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Récupération de tous les canaux avancés Svxlink réussie.");

                return config.AdvanceSvxlinkChannels;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération de tous les canaux avancés Svxlink.");
                throw new Exception("Erreur lors de la récupération de tous les canaux avancés Svxlink.", ex) ;
            }
        }
    }
}
