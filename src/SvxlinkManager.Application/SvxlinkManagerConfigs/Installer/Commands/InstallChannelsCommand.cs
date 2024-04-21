using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record InstallChannelsCommand(Guid ConfigId, IEnumerable<Guid> InstallChannels, string CallSign, string AnnonceCallSign) : IRequest<Unit>;

    public class InstallChannelsCommandHandler : IRequestHandler<InstallChannelsCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly IMediator mediator;
        private readonly ILogger<InstallChannelsCommandHandler> logger;

        public InstallChannelsCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, IMediator mediator, ILogger<InstallChannelsCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<Unit> Handle(InstallChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Début de l'installation des canaux.");

                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var channels = await mediator.Send(new GetAllOriginalChannelsCommand(), cancellationToken);

                foreach (var channelId in request.InstallChannels)
                {
                    var channel = channels.FirstOrDefault(c => c.Id == channelId);
                    if (channel is not null)
                    {
                        channel.CallSign = request.CallSign;
                        channel.ReportCallSign = request.AnnonceCallSign;

                        config.AddSvxlinkChannel(channel);
                        logger.LogInformation("Le canal {channelName} a été installé avec succès.", channel.Name);
                    }
                }

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Les canaux ont été installés avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de l'installation des canaux.");

                throw;
            }
        }
    }

}
