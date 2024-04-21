using MediatR;

using Microsoft.Extensions.Logging;

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
        private readonly ILogger<GetAllEcholinkChannelsQueryHandler> logger;

        public GetAllEcholinkChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllEcholinkChannelsQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<EcholinkChannel>> Handle(GetAllEcholinkChannelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les canaux Echolink.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Récupération de tous les canaux Echolink réussie.");

                return config.EcholinkChannels;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Erreur lors de la récupération de tous les canaux Echolink.");
                throw new Exception("Erreur lors de la récupération de tous les canaux Echolink.",ex);
            }
        }
    }
}
