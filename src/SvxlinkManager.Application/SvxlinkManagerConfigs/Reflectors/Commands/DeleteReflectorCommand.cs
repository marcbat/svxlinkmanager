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
    public record DeleteReflectorCommand(Guid ConfigGuid, Guid ReflectorGuid) : IRequest<Unit>;

    /// <summary>
    /// Gère la commande de suppression d'un réflecteur.
    /// </summary>
    internal class DeleteReflectorCommandHandler : IRequestHandler<DeleteReflectorCommand, Unit>
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
        public async Task<Unit> Handle(DeleteReflectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Suppression du réflecteur.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var reflector = config.Reflectors.SingleOrDefault(r => r.Id == request.ReflectorGuid) ?? throw new SvxlinkManagerException("Impossible de trouver le réflecteur.");

                config.DeleteReflector(request.ReflectorGuid);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Réflecteur supprimé.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du réflecteur.");
                throw new SvxlinkManagerException("Impossible de supprimer le réflecteur.", ex);
            }
        }
    }
}
