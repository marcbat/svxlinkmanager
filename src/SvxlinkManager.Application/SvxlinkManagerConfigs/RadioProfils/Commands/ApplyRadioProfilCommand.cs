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

        private readonly RadioProfilService radioProfilService;
        private readonly ILogger<ApplyRadioProfilCommandHandler> logger;

        public ApplyRadioProfilCommandHandler(RadioProfilService radioProfilService,
                                              ILogger<ApplyRadioProfilCommandHandler> logger)
        {
            this.radioProfilService = radioProfilService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(ApplyRadioProfilCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Application du profil radio.");

            var result = radioProfilService.ApplyRadioProfil(request.ConfigId, request.RadioProfilId);

            logger.LogInformation("Le profil radio a été appliqué avec succès.");

            return Task.FromResult(result); 

        }
    }
}
