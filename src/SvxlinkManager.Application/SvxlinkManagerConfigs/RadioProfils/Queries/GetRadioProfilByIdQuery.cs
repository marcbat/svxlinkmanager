using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries
{
    public record GetRadioProfilByIdQuery(Guid ConfigGuid, Guid ProfilGuid) : IRequest<RadioProfil>;

    internal class GetRadioProfilByIdQueryHandler : IRequestHandler<GetRadioProfilByIdQuery, RadioProfil>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetRadioProfilByIdQueryHandler> logger;

        public GetRadioProfilByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetRadioProfilByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<RadioProfil> Handle(GetRadioProfilByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération du profil radio.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var profil = config.RadioProfils.SingleOrDefault(p => p.Id == request.ProfilGuid);

                if (profil is null)
                    throw new SvxlinkManagerException("Impossible de trouver le profil radio.");

                logger.LogInformation("Profil radio trouvé.");

                return profil;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération du profil radio.");
                throw new SvxlinkManagerException("Impossible de récupérer le profil radio.", ex);
            }
        }
    }
}
