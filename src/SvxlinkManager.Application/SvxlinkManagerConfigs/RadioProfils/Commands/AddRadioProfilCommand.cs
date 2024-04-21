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
                               string SquelchDetection) : IRequest<Guid>;

    internal class AddRadioProfilCommandHandler : IRequestHandler<AddRadioProfilCommand, Guid>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<AddRadioProfilCommandHandler> logger;

        public AddRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                            ILogger<AddRadioProfilCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Guid> Handle(AddRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Ajout d'un nouveau profil radio.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var radioProfilGuid = Guid.NewGuid();
                var radioProfil = new RadioProfil(radioProfilGuid, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection);

                config.AddRadioProfil(radioProfil);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau profil radio a été ajouté avec succès.");

                return radioProfilGuid;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau profil radio.");
                throw new SvxlinkManagerException("Impossible d'ajouter un nouveau profil radio.", ex);
            }
        }
    }
}
