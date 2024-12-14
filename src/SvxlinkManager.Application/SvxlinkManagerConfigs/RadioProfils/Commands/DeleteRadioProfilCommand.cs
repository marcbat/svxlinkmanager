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
        private readonly RadioProfilService radioProfilService;
        private readonly ILogger<DeleteRadioProfilCommandHandler> logger;

        public DeleteRadioProfilCommandHandler(RadioProfilService radioProfilService, ILogger<DeleteRadioProfilCommandHandler> logger)
        {
            this.radioProfilService = radioProfilService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteRadioProfilCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Suppression du profil radio.");

            var result = radioProfilService.DeleteRadioProfil(request.ConfigGuid, request.RadioProfilGuid);

            logger.LogInformation("Profil radio supprimé.");

            return Task.FromResult(result);

        }
    }
}
