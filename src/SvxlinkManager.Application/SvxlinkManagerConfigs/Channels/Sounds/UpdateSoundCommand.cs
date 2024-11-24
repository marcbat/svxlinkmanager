using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Sounds
{

    public record UpdateSoundCommand(Guid ConfigId, Guid ChannelId, string? SoundName, byte[]? SoundFile) : IRequest<Unit>;

    internal class UpdateSoundCommandHandler : IRequestHandler<UpdateSoundCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<UpdateSoundCommandHandler> logger;

        public UpdateSoundCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISoundRepository soundRepository, ILogger<UpdateSoundCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.soundRepository = soundRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateSoundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Execution du handler de la commande UpdateSoundCommand");

                var config = svxlinkManagerConfigRepository.GetConfig(request.ConfigId);



                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'execution de la commande UpdateSoundCommand", ex);
            }
        }
    }
    
}
