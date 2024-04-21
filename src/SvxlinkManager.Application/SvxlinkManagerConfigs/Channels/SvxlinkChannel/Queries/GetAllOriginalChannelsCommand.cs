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
    /// <summary>
    /// Represents a command to get all original channels.
    /// </summary>
    public record GetAllOriginalChannelsCommand : IRequest<IEnumerable<Domain.Entities.SvxlinkChannel>>;

    internal class GetAllOriginalChannelsCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllOriginalChannelsCommand> logger) : IRequestHandler<GetAllOriginalChannelsCommand, IEnumerable<Domain.Entities.SvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllOriginalChannelsCommand> logger = logger;

        /// <summary>
        /// Handles the GetAllOriginalChannelsCommand request.
        /// </summary>
        /// <param name="request">The GetAllOriginalChannelsCommand request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of original channels.</returns>
        public async Task<IEnumerable<Domain.Entities.SvxlinkChannel>> Handle(GetAllOriginalChannelsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetAllOriginalChannelsCommand");

            var channels = await svxlinkManagerConfigRepository.GetAllOriginalChannels();
            logger.LogInformation("GetAllOriginalChannelsCommand: {channels}", channels);

            return channels;
        }
    }
}
