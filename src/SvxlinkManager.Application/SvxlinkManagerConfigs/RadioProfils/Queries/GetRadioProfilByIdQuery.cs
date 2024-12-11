using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries
{
    public record GetRadioProfilByIdQuery(Guid ConfigGuid, Guid ProfilGuid) : IRequest<Validation<Error, RadioProfil>>;

    internal class GetRadioProfilByIdQueryHandler : IRequestHandler<GetRadioProfilByIdQuery, Validation<Error, RadioProfil>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetRadioProfilByIdQueryHandler> logger;

        public GetRadioProfilByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetRadioProfilByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, RadioProfil>> Handle(GetRadioProfilByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération du profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from radioProfil in config.GetRadioProfil(request.ProfilGuid)
                         select radioProfil;

            logger.LogInformation("Profil radio trouvé.");

            return Task.FromResult(result);

        }
    }
}
