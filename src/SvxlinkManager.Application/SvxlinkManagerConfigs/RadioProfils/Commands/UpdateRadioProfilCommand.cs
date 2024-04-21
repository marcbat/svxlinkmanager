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
                               string SquelchDetection) : IRequest<Unit>;

    internal class UpdateRadioProfilCommandHandler : IRequestHandler<UpdateRadioProfilCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private readonly ISoundRepository _soundRepository;
        private readonly ILogger<UpdateRadioProfilCommandHandler> _logger;

        public UpdateRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                               ISoundRepository soundRepository,
                                               ILogger<UpdateRadioProfilCommandHandler> logger)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            _soundRepository = soundRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await _svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                config.DeleteRadioProfil(request.ProfilId);

                var radioProfil = new RadioProfil(request.ProfilId, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection);

                config.AddRadioProfil(radioProfil);

                await _svxlinkManagerConfigRepository.UpdateAsync(config);

                _logger.LogInformation("Un profil radio a été mis à jour avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Impossible de mettre à jour un profil radio.");
                throw new SvxlinkManagerException("Impossible de mettre à jour un profil radio.", ex);
            }
        }
    }
}
