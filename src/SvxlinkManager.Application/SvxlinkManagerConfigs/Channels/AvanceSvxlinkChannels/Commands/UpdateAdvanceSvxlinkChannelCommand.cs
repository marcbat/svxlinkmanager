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
                                        string ModuleTrx, string SoundName, byte[] SoundFile) : IRequest<Unit>;

    internal class UpdateAdvanceSvxlinkChannelCommandHandler : IRequestHandler<UpdateAdvanceSvxlinkChannelCommand, Unit>
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

        public async Task<Unit> Handle(UpdateAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Mise à jour d'un canal avancé Svxlink.");

                var config = await _svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var sound = new Sound($"$/sounds/{request.SoundName}.wav", request.Name, request.SoundFile);
                await _soundRepository.CreateAsyc(sound);

                config.DeleteAdvanceSvxlinkChannel(request.ChannelId);

                var advanceSvxlinkChannel = new AdvanceSvxlinkChannel(request.ChannelId, request.Name, sound.Id, request.SvxlinkConf, request.ModuleDtmfRepeater, request.ModuleEchoLink, request.ModuleFrn, request.ModuleHelp, request.ModuleMetarInfo, request.ModuleParrot, request.ModulePropagationMonitor, request.ModuleSelCallEnc, request.ModuleTclVoiceMail, request.ModuleTrx);

                config.AddAdvanceSvxlinkChannel(advanceSvxlinkChannel);

                await _svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un canal avancé Svxlink a été mis à jour avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible de mettre à jour un canal avancé Svxlink.");
                throw new SvxlinkManagerException("Impossible de mettre à jour un canal avancé Svxlink.", ex);
            }
        }
    }
}
