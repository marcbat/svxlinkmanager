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

    public record GetAllNodeQuery() : IRequest<IEnumerable<Node>>;

    internal class GetAllNodeQueryHandler : IRequestHandler<GetAllNodeQuery, IEnumerable<Node>>
    {
        private readonly ISvxlinkServiceBase svxlinkServiceBase;
        private readonly ILogger<GetAllNodeQueryHandler> logger;

        public GetAllNodeQueryHandler(ISvxlinkServiceBase svxlinkServiceBase, ILogger<GetAllNodeQueryHandler> logger)
        {
            this.svxlinkServiceBase = svxlinkServiceBase;
            this.logger = logger;
        }

        public async Task<IEnumerable<Node>> Handle(GetAllNodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Execution du handler de la query GetAllNodeQuery");

                return svxlinkServiceBase.Nodes;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'execution de la query GetAllNodeQuery", ex);
            }
        }
    }
}
