using LanguageExt;
using LanguageExt.Common;

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
    public record GetAllRadioProfilQuery(Guid ConfigId) : IRequest<Validation<Error, IReadOnlyCollection<RadioProfil>>>;

    internal class GetAllRadioProfilQueryHandler : IRequestHandler<GetAllRadioProfilQuery, Validation<Error, IReadOnlyCollection<RadioProfil>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllRadioProfilQueryHandler> logger;

        public GetAllRadioProfilQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllRadioProfilQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IReadOnlyCollection<RadioProfil>>> Handle(GetAllRadioProfilQuery request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Récupération de tous les profils radio.");

                var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             .Map(config => config.RadioProfiles);

                logger.LogInformation("Tous les profils radio ont été récupérés avec succès.");

                return Task.FromResult(result);
           
        }
    }
} 
 