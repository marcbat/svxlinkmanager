using MediatR;

using Microsoft.Extensions.Logging;

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

    internal class GetAllManagedChannelQueryHandler : IRequestHandler<GetAllManagedChannelQuery, IEnumerable<ManagedChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllManagedChannelQueryHandler> logger;

        public GetAllManagedChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllManagedChannelQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<ManagedChannel>> Handle(GetAllManagedChannelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les canaux managés.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Récupération de tous les canaux managés réussie.");

                return config.GetSvxlinkAndEcholinkChannels();
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la récupération des cannaux managés.");
                throw new Exception("Erreur lors de la récupération des cannaux managés.", ex);
            }
        }
    }
}
