using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands
{
    /// <summary>
    /// Représente une commande pour supprimer un réflecteur.
    /// </summary>
    public record DeleteReflectorCommand(Guid ConfigGuid, Guid ReflectorGuid) : IRequest<Validation<Error, Guid>>;

    /// <summary>
    /// Gère la commande de suppression d'un réflecteur.
    /// </summary>
    internal class DeleteReflectorCommandHandler : IRequestHandler<DeleteReflectorCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteReflectorCommandHandler> logger;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DeleteReflectorCommandHandler"/>.
        /// </summary>
        /// <param name="svxlinkManagerConfigRepository">Le référentiel de configuration du gestionnaire Svxlink.</param>
        public DeleteReflectorCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteReflectorCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gère la commande de suppression d'un réflecteur.
        /// </summary>
        /// <param name="request">La commande de suppression du réflecteur.</param>
        /// <param name="cancellationToken">Le jeton d'annulation.</param>
        /// <returns>Une tâche qui représente l'exécution asynchrone de la commande.</returns>
        public Task<Validation<Error, Guid>> Handle(DeleteReflectorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Suppression du réflecteur.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from _ in config.DeleteReflector(request.ReflectorGuid)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Réflecteur supprimé.");

            return Task.FromResult(result);

        }
    }
}
