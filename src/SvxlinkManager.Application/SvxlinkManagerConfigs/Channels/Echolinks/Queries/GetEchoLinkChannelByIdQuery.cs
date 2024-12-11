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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries
{
    public record GetEchoLinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, EcholinkChannel>>;

    internal class GetEchoLinkChannelByIdQueryHandler : IRequestHandler<GetEchoLinkChannelByIdQuery, Validation<Error, EcholinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetEchoLinkChannelByIdQueryHandler> logger;

        public GetEchoLinkChannelByIdQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetEchoLinkChannelByIdQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, EcholinkChannel>> Handle(GetEchoLinkChannelByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération du EchoLink channel.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                         from channel in config.GetEcholinkChannel(request.ChannelGuid)
                         select channel;

            logger.LogInformation("EchoLink channel trouvé.");

            return Task.FromResult(result);

        }
    }
}
