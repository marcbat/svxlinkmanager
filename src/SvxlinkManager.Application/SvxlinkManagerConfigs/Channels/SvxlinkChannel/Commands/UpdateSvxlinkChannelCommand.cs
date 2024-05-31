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
    public record UpdateSvxlinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name, string Host, string CallSign, string AuthKey, int Port, string ReportCallSign, string SoundName, byte[] SoundFile) : IRequest<Unit>;

    internal class UpdateSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<AddSvxlinkChannelCommandHandler> logger) : IRequestHandler<UpdateSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository = soundRepository;
        private readonly ILogger<AddSvxlinkChannelCommandHandler> logger = logger;

        public async Task<Unit> Handle(UpdateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Mise à jour d'un svxlink channel.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var sound = new Sound($"$/sounds/{request.SoundName}.wav", request.Name, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                config.DeleteSvxlinkChannel(request.ChannelId);

                var svxlinkChannel = new Domain.Entities.SvxlinkChannel(request.ChannelId, request.Name, sound.Id, request.Host, request.Port, request.CallSign, request.AuthKey, request.ReportCallSign);

                config.AddSvxlinkChannel(svxlinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un svxlink channel a été mis à jour avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible de mettre à jour un svxlink channel.");
                throw new SvxlinkManagerException("Impossible de mettre à jour un svxlink channel.", ex);
            }
        }
    }


}
