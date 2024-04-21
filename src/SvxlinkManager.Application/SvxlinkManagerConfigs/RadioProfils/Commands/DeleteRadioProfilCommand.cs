using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands
{
    public record DeleteRadioProfilCommand(Guid ConfigGuid, Guid RadioProfilGuid) : IRequest<Unit>;

    internal class DeleteRadioProfilCommandHandler : IRequestHandler<DeleteRadioProfilCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteRadioProfilCommandHandler> logger;

        public DeleteRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteRadioProfilCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Suppression du profil radio.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var radioProfil = config.RadioProfils.SingleOrDefault(rp => rp.Id == request.RadioProfilGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le profil radio.");

                config.DeleteRadioProfil(request.RadioProfilGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Profil radio supprimé.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du profil radio.");
                throw new SvxlinkManagerException("Impossible de supprimer le profil radio.", ex);
            }
        }
    }
}
