using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record InstallChannelsCommand(Guid ConfigId, IEnumerable<Guid> DeleteChannels, string CallSign, string AnnonceCallSign) : IRequest<Unit>;

    public class InstallChannelsCommandHandler : IRequestHandler<InstallChannelsCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<InstallChannelsCommandHandler> logger;

        public InstallChannelsCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<InstallChannelsCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(InstallChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                config.SvxlinkChannels.Where(c => request.DeleteChannels.Contains(c.Id)).ToList().ForEach(c => config.DeleteSvxlinkChannel(c.Id));

                logger.LogInformation("Les canaux ont été supprimés avec succès.");

                config.SvxlinkChannels.ToList().ForEach(c =>
                {
                    c.CallSign = request.CallSign;
                    c.ReportCallSign = request.AnnonceCallSign;
                });

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
