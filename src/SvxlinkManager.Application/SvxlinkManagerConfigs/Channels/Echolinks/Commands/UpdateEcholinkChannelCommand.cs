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
    public record UpdateEcholinkChannelCommand(Guid ConfigId, Guid ChannelId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description, byte[] SoundFile) : IRequest<Unit>;

    public class UpdateEcholinkChannelCommandHandler : IRequestHandler<UpdateEcholinkChannelCommand, Unit>
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

        public async Task<Unit> Handle(UpdateEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var soundGuid = Guid.NewGuid();
                var sound = new Sound(soundGuid, request.Name, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                config.DeleteEcholinkChannel(request.ChannelId);

                var echolinkChannel = new EcholinkChannel(request.ChannelId, request.Name, soundGuid, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description);

                config.AddEcholinkChannel(echolinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("An echolink channel has been successfully updated.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to update an echolink channel.");
                throw new SvxlinkManagerException("Unable to update an echolink channel.", ex);
            }
        }
    }
}
