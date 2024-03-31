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
    public record GetManagedChannelByIdCommand(Guid ConfigId, Guid ChannelId): IRequest<ManagedChannel>;

    public class GetManagedChannelByIdCommandHandler : IRequestHandler<GetManagedChannelByIdCommand, ManagedChannel>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetManagedChannelByIdCommandHandler> logger;

        public GetManagedChannelByIdCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetManagedChannelByIdCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<ManagedChannel> Handle(GetManagedChannelByIdCommand request, CancellationToken cancellationToken)
        {
            var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

            var channel = config.GetManagedChannel(request.ChannelId);

            if (channel is null)
            {
                logger.LogError("Channel not found.");
                throw new SvxlinkManagerException("Channel not found.");
            }

            logger.LogInformation("Channel found.");

            return channel;
        }
    }
}
