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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Commands
{
    public record AddAdvanceSvxlinkChannelCommand(Guid ConfigId,
                                        string Name,
                                        string SvxlinkConf,
                                        string ModuleDtmfRepeater,
                                        string ModuleEchoLink,
                                        string ModuleFrn,
                                        string ModuleHelp,
                                        string ModuleMetarInfo,
                                        string ModuleParrot,
                                        string ModulePropagationMonitor,
                                        string ModuleSelCallEnc,
                                        string ModuleTclVoiceMail,
                                        string ModuleTrx, string SoundName, byte[] SoundFile) : IRequest<Validation<Error, Guid>>;

    internal class AddAdvanceSvxlinkChannelCommandHandler : IRequestHandler<AddAdvanceSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<AddAdvanceSvxlinkChannelCommandHandler> logger;

        public AddAdvanceSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                     ISoundRepository soundRepository,
                                                     ILogger<AddAdvanceSvxlinkChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.soundRepository = soundRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(AddAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ajout d'un nouveau canal avancé svxlink.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from sound in CreateSound(request.SoundName, request.SoundFile)
                         from channel in AdvanceSvxlinkChannel.Create(Guid.NewGuid(), request.Name, request.SvxlinkConf, request.ModuleDtmfRepeater, request.ModuleEchoLink, request.ModuleFrn, request.ModuleHelp, request.ModuleMetarInfo, request.ModuleParrot, request.ModulePropagationMonitor, request.ModuleSelCallEnc, request.ModuleTclVoiceMail, request.ModuleTrx)
                         from _ in config.AddAdvanceSvxlinkChannel(channel)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Canal avancé svxlink ajouté avec succès.");

            return Task.FromResult(result);
        }

        private Validation<Error, LanguageExt.Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(soundRepository.CreateAsyc);
        }
    }
}
