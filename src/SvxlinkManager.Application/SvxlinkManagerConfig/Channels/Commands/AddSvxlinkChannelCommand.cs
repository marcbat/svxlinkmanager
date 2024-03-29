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

namespace SvxlinkManager.Application.SvxlinkManagerConfig.Channels.Commands
{
    public record AddSvxlinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, int Port, string ReportCallSign, byte[] SoundFile) : IRequest<Unit>;

    public class AddSvxlinkChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                                         ISoundRepository soundRepository,
                                                         ILogger<AddSvxlinkChannelCommandHandler> logger) : IRequestHandler<AddSvxlinkChannelCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository = soundRepository;
        private readonly ILogger<AddSvxlinkChannelCommandHandler> logger = logger;

        public async Task<Unit> Handle(AddSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SvxlinkManagerConfigAggregate config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var soundGuid = Guid.NewGuid();
                var sound = new Sound(soundGuid, request.Name, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                var svxlinkChannel = new SvxlinkChannel(Guid.NewGuid(), request.Name, soundGuid, request.Host, request.CallSign, request.Port, request.ReportCallSign);

                config.AddSvxlinkChannel(svxlinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau svxlink channel a été ajouté avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau svxlink channel.");
                throw new SvxlinkManagerException("Impossible d'ajouter un nouveau svxlink channel.", ex);
            }
        }
    }
}
