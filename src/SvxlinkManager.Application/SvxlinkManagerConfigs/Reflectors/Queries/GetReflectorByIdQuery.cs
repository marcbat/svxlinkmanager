using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Queries
{
    /// <summary>
    /// Représente une requête pour obtenir un réflecteur par son identifiant.
    /// </summary>
    public record GetReflectorByIdQuery(Guid ConfigGuid, Guid ReflectorGuid) : IRequest<Reflector>;

    /// <summary>
    /// Gère la requête pour obtenir un réflecteur par son identifiant.
    /// </summary>
    internal class GetReflectorByIdQueryHandler : IRequestHandler<GetReflectorByIdQuery, Reflector>
    {
        private readonly ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private readonly ILogger<GetReflectorByIdQueryHandler> logger;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="GetReflectorByIdQueryHandler"/>.
        /// </summary>
        /// <param name="svxlinkManagerConfigRepository">Le référentiel de configuration de SvxlinkManager.</param>
        public GetReflectorByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetReflectorByIdQueryHandler> logger)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gère la requête pour obtenir un réflecteur par son identifiant.
        /// </summary>
        /// <param name="request">La requête pour obtenir un réflecteur.</param>
        /// <param name="cancellationToken">Le jeton d'annulation.</param>
        /// <returns>Le réflecteur correspondant à l'identifiant spécifié.</returns>
        public async Task<Reflector> Handle(GetReflectorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération du réflecteur.");

                var config = await _svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var reflector = config.Reflectors.SingleOrDefault(r => r.Id == request.ReflectorGuid);

                if (reflector is null)
                    throw new SvxlinkManagerException("Impossible de trouver le réflecteur.");

                logger.LogInformation("Réflecteur trouvé.");

                return reflector;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération du réflecteur.");
                throw new SvxlinkManagerException("Impossible de récupérer le réflecteur.", ex);
            }
        }
    }
}
