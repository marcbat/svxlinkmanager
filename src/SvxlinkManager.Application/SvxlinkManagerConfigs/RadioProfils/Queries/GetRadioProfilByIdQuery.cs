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
        private readonly RadioProfilService radioProfilService;
        private readonly ILogger<GetRadioProfilByIdQueryHandler> logger;

        public GetRadioProfilByIdQueryHandler(RadioProfilService radioProfilService, ILogger<GetRadioProfilByIdQueryHandler> logger)
        {
            this.radioProfilService = radioProfilService;
            this.logger = logger;
        }

        public Task<Validation<Error, RadioProfil>> Handle(GetRadioProfilByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération du profil radio.");

            var result = radioProfilService.GetRadioProfilById(request.ConfigGuid, request.ProfilGuid);

            logger.LogInformation("Profil radio trouvé.");

            return Task.FromResult(result);

        }
    }
}
