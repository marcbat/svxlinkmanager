using LanguageExt;
using LanguageExt.Common;

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
    public record GetAllReflectorQuery(Guid ConfigId) : IRequest<Validation<Error, IReadOnlyCollection<Reflector>>>;

    internal class GetAllReflectorQueryHandler : IRequestHandler<GetAllReflectorQuery, Validation<Error, IReadOnlyCollection<Reflector>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllReflectorQueryHandler> logger;

        public GetAllReflectorQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllReflectorQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IReadOnlyCollection<Reflector>>> Handle(GetAllReflectorQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération de tous les réflecteurs.");

            var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         .Map(config => config.Reflectors);

            logger.LogInformation("Tous les réflecteurs ont été récupérés avec succès.");

            return Task.FromResult(result);

        }
    }
}
