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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries
{

    public record GetActiveChannelQuery() : IRequest<ChannelBase?>;

    internal class GetActiveChannelQueryHandler : IRequestHandler<GetActiveChannelQuery, ChannelBase?>
    {
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<GetActiveChannelQueryHandler> logger;

        public GetActiveChannelQueryHandler(ISvxlinkServiceBase svxlinkService, ILogger<GetActiveChannelQueryHandler> logger)
        {
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<ChannelBase?> Handle(GetActiveChannelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Execution du handler de la query GetActiveChannelQuery");

                return svxlinkService.ActiveChannel;

            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'execution de la query GetActiveChannelQuery", ex);
            }
        }
    }
}
