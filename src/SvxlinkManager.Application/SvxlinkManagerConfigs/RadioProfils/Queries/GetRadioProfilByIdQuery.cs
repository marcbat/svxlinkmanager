using MediatR;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries
{
    public record GetRadioProfilByIdQuery(Guid ConfigGuid, Guid ProfilGuid) : IRequest<RadioProfil>;

    public class GetRadioProfilByIdQueryHandler : IRequestHandler<GetRadioProfilByIdQuery, RadioProfil>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public GetRadioProfilByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<RadioProfil> Handle(GetRadioProfilByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var profil = config.RadioProfils.SingleOrDefault(p => p.Id == request.ProfilGuid);

                if (profil is null)
                    throw new SvxlinkManagerException("Impossible de trouver le profil radio.");

                return profil;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le profil radio.", ex);
            }
        }
    }
}
