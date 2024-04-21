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
    public record ApplyRadioProfilCommand(Guid ConfigId, Guid RadioProfilId) : IRequest<Unit>;

    internal class ApplyRadioProfilCommandHandler : IRequestHandler<ApplyRadioProfilCommand, Unit>
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

        public async Task<Unit> Handle(ApplyRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Application du profil radio.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var radioProfil = config.RadioProfils.FirstOrDefault(rp => rp.Id == request.RadioProfilId) ?? throw new SvxlinkManagerException("Le profil radio spécifié n'existe pas.");
                
                if (radioProfil.HasSa818)
                    sa818Service.WriteRadioProfile(radioProfil);

                radioProfil.Enable = true;

                config.SetActiveRadioProfile(radioProfil.Id);
                
                await svxlinkManagerConfigRepository.UpdateAsync(config);

                //if(svxlinkService.ActiveChannel is null)
                //    await svxlinkService.

                logger.LogInformation("Le profil radio a été appliqué avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'appliquer le profil radio.");
                throw new SvxlinkManagerException("Impossible d'appliquer le profil radio.", ex);
            }
        }
    }
}
