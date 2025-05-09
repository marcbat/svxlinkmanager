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
    public record AddEcholinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description) : IRequest<Validation<Error, Guid>>;


    internal class AddEcholinkChannelCommandHandler : IRequestHandler<AddEcholinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ChannelService echolinkService;
        private readonly ILogger<AddEcholinkChannelCommandHandler> logger;

        public AddEcholinkChannelCommandHandler(ChannelService channelService,
                                                ILogger<AddEcholinkChannelCommandHandler> logger)
        {
           
            this.echolinkService = channelService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(AddEcholinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Ajout d'un nouveau echolink channel.");

            var result = echolinkService.AddEcholink(request.ConfigId, Guid.NewGuid(), request.Name, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description);

            logger.LogInformation("Un nouveau echolink channel a été ajouté avec succès.");

            return Task.FromResult(result); 

        }

       
    }
}
