using LanguageExt;
using LanguageExt.Common;

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
    public record DeleteManagedChannelCommand(Guid ConfigGuid, Guid ChannelGuid) : IRequest<Validation<Error, Guid>>;

    internal class DeleteManagedChannelCommandHandler : IRequestHandler<DeleteManagedChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<DeleteManagedChannelCommandHandler> logger;

        public DeleteManagedChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<DeleteManagedChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(DeleteManagedChannelCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Suppression du managed channel.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigGuid)
                             from _ in config.DeleteManagedChannel(request.ChannelGuid)
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Managed channel supprimé.");

                return Task.FromResult(result);
            
        }
    }
}
