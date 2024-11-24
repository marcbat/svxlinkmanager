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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries
{
    public record GetAllSvxlinChannelQuery(Guid ConfigId): IRequest<Validation<Error, IReadOnlyCollection<Domain.Entities.SvxlinkChannel>>>;

    internal class GetAllSvxlinChannelQueryHandler : IRequestHandler<GetAllSvxlinChannelQuery, Validation<Error, IReadOnlyCollection<Domain.Entities.SvxlinkChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllSvxlinChannelQueryHandler> logger;

        public GetAllSvxlinChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllSvxlinChannelQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IReadOnlyCollection<Domain.Entities.SvxlinkChannel>>> Handle(GetAllSvxlinChannelQuery request, CancellationToken cancellationToken)
        {
                logger.LogInformation("Récupération de tous les canaux Svxlink.");

                var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                    .Map(config => config.SvxlinkChannels);

                logger.LogInformation("Récupération de tous les canaux Svxlink réussie.");

                return Task.FromResult(result);
            
        }
    }
}
