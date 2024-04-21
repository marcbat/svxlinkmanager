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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries
{
    public record GetAllRadioProfilQuery(Guid ConfigId) : IRequest<IEnumerable<RadioProfil>>;

    internal class GetAllRadioProfilQueryHandler : IRequestHandler<GetAllRadioProfilQuery, IEnumerable<RadioProfil>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllRadioProfilQueryHandler> logger;

        public GetAllRadioProfilQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllRadioProfilQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<RadioProfil>> Handle(GetAllRadioProfilQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les profils radio.");
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Tous les profils radio ont été récupérés avec succès.");

                return config.RadioProfils;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération de tous les profils radio.");
                throw new Exception("Erreur lors de la récupération de tous les profils radio.", ex);
            }
        }
    }
}
