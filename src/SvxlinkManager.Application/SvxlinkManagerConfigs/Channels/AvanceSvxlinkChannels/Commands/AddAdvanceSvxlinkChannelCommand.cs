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
                                        string ModuleTrx, string SoundName, byte[] SoundFile) : IRequest<Guid>;

    internal class AddAdvanceSvxlinkChannelCommandHandler : IRequestHandler<AddAdvanceSvxlinkChannelCommand, Guid>
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

        public async Task<Guid> Handle(AddAdvanceSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Ajout d'un nouveau canal avancé svxlink.");

                SvxlinkManagerConfigAggregate config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var sound = new Sound($"$/sounds/{request.SoundName}", request.SoundName, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                var advanceSvxlinkchannelGuid = Guid.NewGuid();
                var advanceSvxlinkChannel = new AdvanceSvxlinkChannel(advanceSvxlinkchannelGuid, request.Name, request.SvxlinkConf, request.ModuleDtmfRepeater, request.ModuleEchoLink, request.ModuleFrn, request.ModuleHelp, request.ModuleMetarInfo, request.ModuleParrot, request.ModulePropagationMonitor, request.ModuleSelCallEnc, request.ModuleTclVoiceMail, request.ModuleTrx);

                config.AddAdvanceSvxlinkChannel(advanceSvxlinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau canal avancé svxlink a été ajouté avec succès.");

                return advanceSvxlinkchannelGuid;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau canal avancé svxlink.");
                throw new SvxlinkManagerException("Impossible d'ajouter un nouveau canal avancé svxlink.", ex);
            }
        }
    }
}
