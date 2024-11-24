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
    public record AddSvxlinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, string AuthKey, int Port, string ReportCallSign, string SoundName, byte[] SoundFile) : IRequest<Validation<Error, Guid>>;

    internal class AddSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<AddSvxlinkChannelCommandHandler> logger) : IRequestHandler<AddSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository = soundRepository;
        private readonly ILogger<AddSvxlinkChannelCommandHandler> logger = logger;

        public Task<Validation<Error, Guid>> Handle(AddSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                       from channel in Domain.Entities.SvxlinkChannel.Create(Guid.NewGuid(), request.Name, request.Host, request.Port, request.CallSign, request.AuthKey, request.ReportCallSign)
                       from sound in CreateSound(request.SoundName, request.SoundFile)
                       from _ in svxlinkManagerConfigRepository.UpdateAsync(config)
                       select config.Id;

            return Task.FromResult(result);
        }

        private Validation<Error, LanguageExt.Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(soundRepository.CreateAsyc);
        }
    }
}
