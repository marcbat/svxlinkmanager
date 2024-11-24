using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands
{
    public record DeleteRadioProfilCommand(Guid ConfigGuid, Guid RadioProfilGuid) : IRequest<Validation<Error, Guid>>;

    internal class DeleteRadioProfilCommandHandler : IRequestHandler<DeleteRadioProfilCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteRadioProfilCommandHandler> logger;

        public DeleteRadioProfilCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteRadioProfilCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteRadioProfilCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Suppression du profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from radioProfil in config.GetRadioProfil(request.RadioProfilGuid)
                         from _ in config.DeleteRadioProfil(request.RadioProfilGuid)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Profil radio supprimé.");

            return Task.FromResult(result);

        }
    }
}
