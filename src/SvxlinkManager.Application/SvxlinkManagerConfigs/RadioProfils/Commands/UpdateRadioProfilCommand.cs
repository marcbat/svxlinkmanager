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
    public record UpdateRadioProfilCommand(Guid ConfigId, Guid ProfilId, string Name,
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

    internal class UpdateRadioProfilCommandHandler : IRequestHandler<UpdateRadioProfilCommand, Validation<Error, Guid>>
    {
        private readonly RadioProfilService radioProfilService;
        private readonly ILogger<UpdateRadioProfilCommandHandler> logger;

        public UpdateRadioProfilCommandHandler(RadioProfilService radioProfilService,  ILogger<UpdateRadioProfilCommandHandler> logger)
        {
            this.radioProfilService = radioProfilService;
            this.logger = logger;
        }

        public Task<Validation<Error,Guid>> Handle(UpdateRadioProfilCommand request, CancellationToken cancellationToken)
        {
           
                logger.LogInformation("Mise à jour d'un profil radio.");

                var result = radioProfilService.UpdateRadioProfil(request.ConfigId, request.ProfilId, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection);

            logger.LogInformation("Un profil radio a été mis à jour avec succès.");

                return Task.FromResult(result);
            
        }
    }
}
