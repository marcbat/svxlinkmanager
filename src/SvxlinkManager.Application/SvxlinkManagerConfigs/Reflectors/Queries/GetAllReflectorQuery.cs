using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Queries
{
    public record GetAllReflectorQuery(Guid ConfigId) : IRequest<IEnumerable<Reflector>>;

    internal class GetAllReflectorQueryHandler : IRequestHandler<GetAllReflectorQuery, IEnumerable<Reflector>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllReflectorQueryHandler> logger;

        public GetAllReflectorQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllReflectorQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<Reflector>> Handle(GetAllReflectorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération de tous les réflecteurs.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                logger.LogInformation("Tous les réflecteurs ont été récupérés avec succès.");

                return config.Reflectors;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération de tous les réflecteurs.");
                throw new Exception("Erreur lors de la récupération de tous les réflecteurs.", ex);
            }
        }
    }
}
