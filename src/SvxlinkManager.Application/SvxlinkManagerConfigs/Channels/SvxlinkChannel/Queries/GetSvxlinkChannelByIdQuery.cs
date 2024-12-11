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
    public record GetSvxlinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, Domain.Entities.SvxlinkChannel>>;

    internal class GetSvxlinkChannelByIdQueryHandler : IRequestHandler<GetSvxlinkChannelByIdQuery, Validation<Error, Domain.Entities.SvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetSvxlinkChannelByIdQueryHandler> logger;

        public GetSvxlinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetSvxlinkChannelByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Domain.Entities.SvxlinkChannel>> Handle(GetSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
           
                logger.LogInformation("Récupération du svxlink channel.");

                var result = svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                    .Bind(config => config.GetSvxlinkChannel(request.ChannelGuid));

                logger.LogInformation("Svxlink channel trouvé.");

                return Task.FromResult(result);
           
        }
    }
}
