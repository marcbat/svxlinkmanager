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
    public record AddSvxlinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, string AuthKey, int Port, string ReportCallSign, byte[] SoundFile) : IRequest<Guid>;

    internal class AddSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<AddSvxlinkChannelCommandHandler> logger) : IRequestHandler<AddSvxlinkChannelCommand, Guid>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository = soundRepository;
        private readonly ILogger<AddSvxlinkChannelCommandHandler> logger = logger;

        public async Task<Guid> Handle(AddSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Ajout d'un nouveau svxlink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var soundGuid = Guid.NewGuid();
                var sound = new Sound(soundGuid, request.Name, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                var svxlinkchannelGuid = Guid.NewGuid();
                var svxlinkChannel = new Domain.Entities.SvxlinkChannel(svxlinkchannelGuid, request.Name, soundGuid, request.Host, request.Port, request.CallSign, request.AuthKey, request.ReportCallSign);

                config.AddSvxlinkChannel(svxlinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau svxlink channel a été ajouté avec succès.");

                return svxlinkchannelGuid;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau svxlink channel.");
                throw new SvxlinkManagerException("Impossible d'ajouter un nouveau svxlink channel.", ex);
            }
        }
    }
}
