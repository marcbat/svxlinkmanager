using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record ActivateSvxlinkChannelCommand(Guid ConfigId, Guid IdSvxlinkChannel) : IRequest<Unit>;

    internal class ActivateChannelCommandHandler : IRequestHandler<ActivateSvxlinkChannelCommand, Unit>
    {
        private readonly string applicationPath = Directory.GetCurrentDirectory();
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISvxlinkServiceBase svxlinkService;
        private readonly IIniService iniService;
        private readonly ILogger<ActivateChannelCommandHandler> logger;

        public ActivateChannelCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISvxlinkServiceBase svxlinkService, IIniService iniService, ILogger<ActivateChannelCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.svxlinkService = svxlinkService;
            this.iniService = iniService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(ActivateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);
                var channel = config.SvxlinkChannels.SingleOrDefault(x => x.Id == request.IdSvxlinkChannel) ?? throw new Exception("Svxlink Channel non trouvé.");
                var url = new UriBuilder("http", channel.Host, channel.Port).Uri;

                logger.LogInformation("Restart salon.");

                // Stop svxlink
                svxlinkService.StopSvxlink();
                logger.LogInformation("Salon déconnecté");

                RadioProfil radioProfile = config.GetActiveRadioProfile() ?? throw new Exception("Profil radio non trouvé.");

                logger.LogInformation("Profil radio actuel récupéré.");

                Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");
                File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", svxlinkManagerConfigRepository.GetDefaultSvxlinkConfig());

                var global = new Dictionary<string, string>
                  {
                    { "LOGICS", "SimplexLogic,ReflectorLogic" }
                  };
                            var simplexlogic = new Dictionary<string, string> {
                    { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},
                    { "CALLSIGN", channel.ReportCallSign},
                    { "REPORT_CTCSS", radioProfile.RxCtCss}
                  };
                            var rx = new Dictionary<string, string>
                  {
                    {"SQL_DET", radioProfile.SquelchDetection },
                    {"CTCSS_FQ", radioProfile.RxCtCss }
                  };
                            var tx = new Dictionary<string, string>
                  {
                    {"CTCSS_FQ", radioProfile.TxCtcss }
                  };
                            var ReflectorLogic = new Dictionary<string, string>
                  {
                    {"CALLSIGN", channel.CallSign },
                    {"HOST", channel.Host },
                    {"AUTH_KEY",channel.AuthKey },
                    {"PORT" ,channel.Port.ToString()}
                  };
                            var parameters = new Dictionary<string, Dictionary<string, string>>
                  {
                    {"GLOBAL", global },
                    {"SimplexLogic", simplexlogic },
                    {"Rx1", rx},
                    { "Tx1", tx },
                    {"ReflectorLogic" , ReflectorLogic}
                  };

                iniService.ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.conf", parameters);
                logger.LogInformation("Remplacement du contenu svxlink.conf");

                ReplaceSoundFile(channel);

                // Lance svxlink
                svxlinkService.StartSvxlink(channel, pidFile: "/var/run/svxlink.pid", runAs: "root", configFile: $"{applicationPath}/SvxlinkConfig/svxlink.conf");
                logger.LogInformation($"Le channel {channel.Name} est connecté.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de l'activation du channel svxlink.");
                throw new Exception("Erreur lors de l'activation du channel svxlink.", ex);
            }
        }

        protected virtual void ReplaceSoundFile(ManagedChannel channel)
        {
            logger.LogInformation("Remplacement du fichier wav d'annonce.");

            if (!Directory.Exists("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager"))
                Directory.CreateDirectory("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager");

            logger.LogInformation("Création du répertoire de son.");

            //if (!string.IsNullOrEmpty(channel.Sound.SoundName))
            //{
            //    File.Delete("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav");
            //    File.WriteAllBytes("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav", channel.Sound.SoundFile);
            //}
        }

    }


}
