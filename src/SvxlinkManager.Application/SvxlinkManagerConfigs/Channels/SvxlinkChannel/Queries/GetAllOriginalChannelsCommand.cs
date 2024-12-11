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
    /// <summary>
    /// Represents a command to get all original channels.
    /// </summary>
    public record GetAllOriginalChannelsCommand : IRequest<Validation<Error, List<Domain.Entities.SvxlinkChannel>>>;

    internal class GetAllOriginalChannelsCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAllOriginalChannelsCommand> logger) : IRequestHandler<GetAllOriginalChannelsCommand, Validation<Error, List<Domain.Entities.SvxlinkChannel>>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ILogger<GetAllOriginalChannelsCommand> logger = logger;

        /// <summary>
        /// Handles the GetAllOriginalChannelsCommand request.
        /// </summary>
        /// <param name="request">The GetAllOriginalChannelsCommand request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of original channels.</returns>
        public Task<Validation<Error, List<Domain.Entities.SvxlinkChannel>>> Handle(GetAllOriginalChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Recupéreation des configuration de Channel originals");

                var result = svxlinkManagerConfigRepository.GetAllOriginalChannels();

                logger.LogInformation("Recupération des configuration de Channel originals réussie.");
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération des configuration de Channel originals.");
                throw new Exception("Erreur lors de la récupération des configuration de Channel originals.", ex);
            }
        }
    }
}
