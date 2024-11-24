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
    public record GetAvanceSvxlinkChannelByIdQuery(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, AdvanceSvxlinkChannel>>;

    internal class GetAvanceSvxlinkChannelQueryHandler : IRequestHandler<GetAvanceSvxlinkChannelByIdQuery, Validation<Error, AdvanceSvxlinkChannel>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<GetAvanceSvxlinkChannelQueryHandler> logger;

        public GetAvanceSvxlinkChannelQueryHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<GetAvanceSvxlinkChannelQueryHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, AdvanceSvxlinkChannel>> Handle(GetAvanceSvxlinkChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Récupération du canal avancé Svxlink.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                             from channel in config.GetAdvanceSvxlinkChannel(request.ChannelGuid)
                             select channel;

                logger.LogInformation("Récupération du canal avancé Svxlink réussie.");

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération du canal avancé Svxlink.");
                throw new SvxlinkManagerException("Impossible de récupérer le canal avancé Svxlink.", ex);
            }
        }
    }
}
