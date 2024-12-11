using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands
{
    public record ApplyRadioProfilCommand(Guid ConfigId, Guid RadioProfilId) : IRequest<Validation<Error, Guid>>;

    internal class ApplyRadioProfilCommandHandler : IRequestHandler<ApplyRadioProfilCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISa818Service sa818Service;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<ApplyRadioProfilCommandHandler> logger;

        public ApplyRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISa818Service sa818Service, ISvxlinkServiceBase svxlinkService,
                                              ILogger<ApplyRadioProfilCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.sa818Service = sa818Service;
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(ApplyRadioProfilCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Application du profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from radioProfile in config.GetRadioProfil(request.RadioProfilId)
                         from _ in WriteRadioProfil(radioProfile)
                         from __ in config.SetActiveRadioProfile(radioProfile.Id)
                         from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Le profil radio a été appliqué avec succès.");

            return Task.FromResult(result);

        }

        private Validation<Error, LanguageExt.Unit> WriteRadioProfil(RadioProfil radioProfil)
        {
            if (radioProfil.HasSa818)
                return sa818Service.WriteRadioProfile(radioProfil);

            return LanguageExt.Unit.Default;
        }
    }
}
