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
    public record AddEcholinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description, string SoundName, byte[] SoundFile) : IRequest<Validation<Error, Guid>>;


    internal class AddEcholinkChannelCommandHandler : IRequestHandler<AddEcholinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<AddEcholinkChannelCommandHandler> logger;

        public AddEcholinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                ISoundRepository soundRepository,
                                                ILogger<AddEcholinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.soundRepository = soundRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(AddEcholinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Ajout d'un nouveau echolink channel.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from Sound in CreateSound(request.SoundName, request.SoundFile)
                         from channel in EcholinkChannel.Create(Guid.NewGuid(), request.Name, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description)
                         from _ in config.AddEcholinkChannel(channel)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Un nouveau echolink channel a été ajouté avec succès.");

            return Task.FromResult(result);

        }

        private Validation<Error, LanguageExt.Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(soundRepository.CreateAsyc);
        }
    }
}
