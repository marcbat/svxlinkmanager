using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{

    public record DisconnectChannelCommand() : IRequest<Unit>;

    internal class DisconnectChannelCommandHandler : IRequestHandler<DisconnectChannelCommand, Unit>
    {
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly ILogger<DisconnectChannelCommandHandler> logger;

        public DisconnectChannelCommandHandler(ISvxlinkServiceBase svxlinkService, ILogger<DisconnectChannelCommandHandler> logger)
        {
            this.svxlinkService = svxlinkService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DisconnectChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Execution du handler de la commande DisconnectChannelCommand");

                svxlinkService.StopSvxlink();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'execution de la commande DisconnectChannelCommand", ex);
            }
        }
    }
}
