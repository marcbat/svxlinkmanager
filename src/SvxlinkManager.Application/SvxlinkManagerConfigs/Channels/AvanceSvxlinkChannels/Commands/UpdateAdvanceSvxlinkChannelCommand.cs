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
    public record UpdateAdvanceSvxlinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name,
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

    internal class UpdateAdvanceSvxlinkChannelCommandHandler : IRequestHandler<UpdateAdvanceSvxlinkChannelCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private readonly ISoundRepository _soundRepository;
        private readonly ILogger<UpdateAdvanceSvxlinkChannelCommandHandler> logger;

        public UpdateAdvanceSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<UpdateAdvanceSvxlinkChannelCommandHandler> logger)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            _soundRepository = soundRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(UpdateAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Mise à jour d'un canal avancé Svxlink.");

            var result = from config in _svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from sound in CreateSound(request.SoundName, request.SoundFile)
                         from _ in config.DeleteAdvanceSvxlinkChannel(request.ChannelId)
                         from channel in AdvanceSvxlinkChannel.Create(request.ChannelId, request.Name, request.SvxlinkConf, request.ModuleDtmfRepeater, request.ModuleEchoLink, request.ModuleFrn, request.ModuleHelp, request.ModuleMetarInfo, request.ModuleParrot, request.ModulePropagationMonitor, request.ModuleSelCallEnc, request.ModuleTclVoiceMail, request.ModuleTrx)
                         from __ in config.AddAdvanceSvxlinkChannel(channel)
                         from ___ in _svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Un canal avancé Svxlink a été mis à jour avec succès.");

            return Task.FromResult(result);

        }

        private Validation<Error, LanguageExt.Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(_soundRepository.CreateAsyc);
        }
    }
}
