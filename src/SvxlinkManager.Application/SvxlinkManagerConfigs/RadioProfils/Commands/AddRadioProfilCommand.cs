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
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands
{
    public record AddRadioProfilCommand(Guid ConfigId, string Name,
                               string RxFrequency,
                               string TxFrequency,
                               string Squelch,
                               string TxCtcss,
                               string RxCtCss,
                               string Volume,
                               string PreEmph,
                               string HighPass,
                               string LowPass,
                               string SquelchDetection) : IRequest<Validation<Error, Guid>>;

    internal class AddRadioProfilCommandHandler : IRequestHandler<AddRadioProfilCommand, Validation<Error, Guid>>
    {
        private readonly RadioProfilService radioProfilService;
        private readonly ILogger<AddRadioProfilCommandHandler> logger;

        public AddRadioProfilCommandHandler(RadioProfilService radioProfilService,
                                            ILogger<AddRadioProfilCommandHandler> logger)
        {
            this.radioProfilService = radioProfilService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(AddRadioProfilCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ajout d'un nouveau profil radio.");

            var result = radioProfilService.AddRadioProfil(request.ConfigId,
                                                            request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection);

            logger.LogInformation("Un nouveau profil radio a été ajouté avec succès.");

            return Task.FromResult(result);
        }
    }
}
