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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Queries
{
    public record GetAllAdvanceChannelsQuery(Guid ConfigId) : IRequest<Validation<Error, IReadOnlyCollection<AdvanceSvxlinkChannel>>>;

    internal class GetAllAdvanceChannelsQueryHandler : IRequestHandler<GetAllAdvanceChannelsQuery, Validation<Error, IReadOnlyCollection<AdvanceSvxlinkChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllAdvanceChannelsQueryHandler> logger;

        public GetAllAdvanceChannelsQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllAdvanceChannelsQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, IReadOnlyCollection<AdvanceSvxlinkChannel>>> Handle(GetAllAdvanceChannelsQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération de tous les canaux avancés Svxlink.");

            Validation<Error, IReadOnlyCollection<AdvanceSvxlinkChannel>> result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                                                                                   select config.AdvanceSvxlinkChannels;

            logger.LogInformation("Récupération de tous les canaux avancés Svxlink réussie.");

            return Task.FromResult(result);

        }
    }
}
