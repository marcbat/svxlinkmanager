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
    public record UpdateEcholinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description, string SoundName, byte[] SoundFile) : IRequest<Validation<Error, Guid>>;

    internal class UpdateEcholinkChannelCommandHandler : IRequestHandler<UpdateEcholinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<UpdateEcholinkChannelCommandHandler> logger;

        public UpdateEcholinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                   ISoundRepository soundRepository,
                                                   ILogger<UpdateEcholinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.soundRepository = soundRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(UpdateEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Mise à jour du canal Echolink");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from sound in CreateSound(request.SoundName, request.SoundFile)
                             from _ in config.DeleteEcholinkChannel(request.ChannelId)
                             from channel in EcholinkChannel.Create(request.ChannelId, request.Name, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description)
                             from __ in config.AddEcholinkChannel(channel)
                             from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Echolink channel updated successfully.");

                return Task.FromResult(result);
            
        }

        private Validation<Error, LanguageExt.Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(soundRepository.CreateAsyc);
        }
    }
}
