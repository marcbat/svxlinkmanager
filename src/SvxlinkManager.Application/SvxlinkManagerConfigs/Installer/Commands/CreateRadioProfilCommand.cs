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
                               string SquelchDetection) : IRequest<Unit>;

    internal class CreateRadioProfilCommandHandler : IRequestHandler<CreateRadioProfilCommand, Unit>
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

        public async Task<Unit> Handle(CreateRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Début de la création d'un nouveau profil radio.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                config.DeleteAllRadioProfils();

                var radioProfilGuid = Guid.NewGuid();
                var radioProfil = new RadioProfil(radioProfilGuid, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection);

                radioProfil.Enable = true;

                config.AddRadioProfil(radioProfil);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau profil radio a été ajouté avec succès.");

                sa818Service.WriteRadioProfile(radioProfil);

                logger.LogInformation("Le profil radio a été écrit avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Une erreur s'est produite lors de la création du profil radio.");
                throw new SvxlinkManagerException("Une erreur s'est produite lors de la création du profil radio.", ex);
            }
        }
    }
}
