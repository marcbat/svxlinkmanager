using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands
{
    public record UpdateEcholinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description) : IRequest<Validation<Error, Guid>>;

    internal class UpdateEcholinkChannelCommandHandler : IRequestHandler<UpdateEcholinkChannelCommand, Validation<Error, Guid>>
    {

        private readonly EcholinkService echolinkService;
        private readonly ILogger<UpdateEcholinkChannelCommandHandler> logger;

        public UpdateEcholinkChannelCommandHandler(EcholinkService echolinkService,
                                                   ILogger<UpdateEcholinkChannelCommandHandler> logger)
        {
            this.echolinkService = echolinkService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(UpdateEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Mise à jour du canal Echolink");

                var result = echolinkService.UpdateEcholink(request.ConfigId, request.ChannelId, request.Name, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description);

            logger.LogInformation("Echolink channel updated successfully.");

                return Task.FromResult(result);
            
        }

        
    }
}
