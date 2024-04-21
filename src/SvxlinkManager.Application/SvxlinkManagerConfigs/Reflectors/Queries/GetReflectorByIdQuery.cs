using MediatR;

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

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="GetReflectorByIdQueryHandler"/>.
        /// </summary>
        /// <param name="svxlinkManagerConfigRepository">Le référentiel de configuration de SvxlinkManager.</param>
        public GetReflectorByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
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
                var config = await _svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigGuid);

                var reflector = config.Reflectors.SingleOrDefault(r => r.Id == request.ReflectorGuid);

                if (reflector is null)
                    throw new SvxlinkManagerException("Impossible de trouver le réflecteur.");

                return reflector;
            }
            catch (Exception ex)
            {
                throw new SvxlinkManagerException("Impossible de récupérer le réflecteur.", ex);
            }
        }
    }
}
