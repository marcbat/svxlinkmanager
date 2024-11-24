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
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries
{
    public record GetAllManagedChannelQuery(Guid ConfigId) : IRequest<Validation<Error, IEnumerable<ManagedChannel>>>;

    internal class GetAllManagedChannelQueryHandler : IRequestHandler<GetAllManagedChannelQuery, Validation<Error, IEnumerable<ManagedChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllManagedChannelQueryHandler> logger;

        public GetAllManagedChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllManagedChannelQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IEnumerable<ManagedChannel>>> Handle(GetAllManagedChannelQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération de tous les canaux managés.");

            var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                .Bind(config => config.GetSvxlinkAndEcholinkChannels());

            logger.LogInformation("Récupération de tous les canaux managés réussie.");

            return Task.FromResult(result);

        }
    }
}
