using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries
{
    public record GetAllEcholinkChannelsQuery(Guid ConfigId) : IRequest<Validation<Error, IReadOnlyCollection<EcholinkChannel>>>;

    internal class GetAllEcholinkChannelsQueryHandler : IRequestHandler<GetAllEcholinkChannelsQuery, Validation<Error, IReadOnlyCollection<EcholinkChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllEcholinkChannelsQueryHandler> logger;

        public GetAllEcholinkChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllEcholinkChannelsQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IReadOnlyCollection<EcholinkChannel>>> Handle(GetAllEcholinkChannelsQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération de tous les canaux Echolink.");

            var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                                    .Map(config => config.EcholinkChannels);

            logger.LogInformation("Récupération de tous les canaux Echolink réussie.");

            return Task.FromResult(result);

        }
    }
}
