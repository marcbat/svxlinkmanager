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
        private readonly ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private readonly ISoundRepository _soundRepository;
        private readonly ILogger<UpdateRadioProfilCommandHandler> logger;

        public UpdateRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                               ISoundRepository soundRepository,
                                               ILogger<UpdateRadioProfilCommandHandler> logger)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            _soundRepository = soundRepository;
            this.logger = logger;
        }

        public Task<Validation<Error,Guid>> Handle(UpdateRadioProfilCommand request, CancellationToken cancellationToken)
        {
           
                logger.LogInformation("Mise à jour d'un profil radio.");

                var result = from config in _svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from _ in config.DeleteRadioProfil(request.ProfilId)
                             from radioProfil in RadioProfil.Create(request.ProfilId, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection)
                             from __ in config.AddRadioProfil(radioProfil)
                             from ___ in _svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Un profil radio a été mis à jour avec succès.");

                return Task.FromResult(result);
            
        }
    }
}
