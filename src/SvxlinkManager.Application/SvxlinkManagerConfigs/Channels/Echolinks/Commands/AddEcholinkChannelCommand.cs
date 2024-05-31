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
    public record AddEcholinkChannelCommand(Guid ConfigId, string Name, string Host, string CallSign, string Password, string SysopName, string Location, int MaxQso, string Description, string SoundName, byte[] SoundFile) : IRequest<Guid>;


    internal class AddEcholinkChannelCommandHandler : IRequestHandler<AddEcholinkChannelCommand, Guid>
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

        public async Task<Guid> Handle(AddEcholinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Ajout d'un nouveau echolink channel.");

                SvxlinkManagerConfigAggregate config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

             
                var sound = new Sound($"$/sounds/{request.SoundName}.wav", request.Name, request.SoundFile);
                await soundRepository.CreateAsyc(sound);

                var echolinkChannelGuid = Guid.NewGuid();
                var echolinkChannel = new EcholinkChannel(echolinkChannelGuid, request.Name, sound.Id, request.Host, request.CallSign, request.Password, request.SysopName, request.Location, request.MaxQso, request.Description);

                config.AddEcholinkChannel(echolinkChannel);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau echolink channel a été ajouté avec succès.");

                return echolinkChannelGuid;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau echolink channel.");
                throw new SvxlinkManagerException("Impossible d'ajouter un nouveau echolink channel.", ex);
            }
        }
    }
}
