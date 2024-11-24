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
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record CreateRadioProfilCommand(Guid ConfigId, string Name,
                               string RxFrequency,
                               string TxFrequency,
                               string Squelch,
                               string TxCtcss,
                               string RxCtCss,
                               string Volume,
                               string PreEmph,
                               string HighPass,
                               string LowPass,
                               string SquelchDetection) : IRequest<Validation<Error, RadioProfil>>;

    internal class CreateRadioProfilCommandHandler : IRequestHandler<CreateRadioProfilCommand, Validation<Error, RadioProfil>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISa818Service sa818Service;
        private readonly ILogger<CreateRadioProfilCommandHandler> logger;

        public CreateRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISa818Service sa818Service, ILogger<CreateRadioProfilCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.sa818Service = sa818Service;
            this.logger = logger;
        }

        public Task<Validation<Error, RadioProfil>> Handle(CreateRadioProfilCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Début de la création d'un nouveau profil radio.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from _ in config.DeleteAllRadioProfils()
                             from radioProfile in RadioProfil.Create(Guid.NewGuid(), request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection)
                             from __ in EnableRadioProfile(radioProfile)
                             from ___ in config.AddRadioProfil(radioProfile)
                             from ____ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             from _____ in sa818Service.WriteRadioProfile(radioProfile)
                             select radioProfile;
                
                logger.LogInformation("Le profil radio a été écrit avec succès.");

                return Task.FromResult(result);
           
        }

        private static Validation<Error, LanguageExt.Unit> EnableRadioProfile(RadioProfil radioProfil)
        {
            radioProfil.Enable = true;

            return LanguageExt.Unit.Default;
        }
    }
}
