using MediatR;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands
{
    public record DeleteRadioProfilCommand(Guid ConfigGuid, Guid RadioProfilGuid) : IRequest<Unit>;

    public class DeleteRadioProfilCommandHandler : IRequestHandler<DeleteRadioProfilCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;

        public DeleteRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        }

        public async Task<Unit> Handle(DeleteRadioProfilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var radioProfil = config.RadioProfils.SingleOrDefault(rp => rp.Id == request.RadioProfilGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le profil radio.");

                config.DeleteRadioProfil(request.RadioProfilGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de supprimer le profil radio.", ex);
            }
        }
    }
}
