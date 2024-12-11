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
    public record GetManagedChannelByIdQuery(Guid ConfigId, Guid ChannelId) : IRequest<Validation<Error, Option<ManagedChannel>>>;

    internal class GetManagedChannelByIdCommandHandler : IRequestHandler<GetManagedChannelByIdQuery, Validation<Error, Option<ManagedChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetManagedChannelByIdCommandHandler> logger;

        public GetManagedChannelByIdCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetManagedChannelByIdCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Option<ManagedChannel>>> Handle(GetManagedChannelByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Récupération de canal managé.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from channel in config.GetManagedChannel(request.ChannelId)
                         select channel;

            logger.LogInformation("Channel found.");

            return Task.FromResult(result);

        }
    }
}
