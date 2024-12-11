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
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands
{
    public record UpdateSvxlinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name, string Host, string CallSign, string AuthKey, int Port, string ReportCallSign, string? SoundName, byte[]? SoundFile) : IRequest<Validation<Error, Guid>>;

    internal class UpdateSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<AddSvxlinkChannelCommandHandler> logger) : IRequestHandler<UpdateSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository = soundRepository;
        private readonly ILogger<AddSvxlinkChannelCommandHandler> logger = logger;

        public Task<Validation<Error, Guid>> Handle(UpdateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Mise à jour d'un svxlink channel.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from _ in config.UpdateSvxlinkChannel(request.ChannelId, request.Name, request.Host, request.CallSign, request.AuthKey, request.Port, request.ReportCallSign)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Un svxlink channel a été mis à jour avec succès.");

            return Task.FromResult(result);

        }
    }
}
