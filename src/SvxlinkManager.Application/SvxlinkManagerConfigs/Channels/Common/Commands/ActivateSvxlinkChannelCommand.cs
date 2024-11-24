using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands
{
    public record ActivateSvxlinkChannelCommand(Guid ConfigId, Guid IdSvxlinkChannel) : IRequest<Validation<Error, Guid>>;

    internal class ActivateChannelCommandHandler : IRequestHandler<ActivateSvxlinkChannelCommand, Validation<Error, Guid>>
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

        public Task<Validation<Error, Guid>> Handle(ActivateSvxlinkChannelCommand request, CancellationToken cancellationToken)
        {

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from channel in config.GetSvxlinkChannel(request.IdSvxlinkChannel)
                         from _ in svxlinkService.StopSvxlink()
                         from radioProfile in config.GetActiveRadioProfile()
                         from __ in WriteDefaultSvxlinkConfig()
                         from parameters in CreateParametersDictionnary(channel, radioProfile)
                         from ___ in iniService.ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.conf", parameters)
                         from ____ in ReplaceSoundFile(channel)
                         from _____ in svxlinkService.StartSvxlink(channel, pidFile: "/var/run/svxlink.pid", runAs: "root", configFile: $"{applicationPath}/SvxlinkConfig/svxlink.conf")
                         select config.Id;


            return Task.FromResult(result);

        }

        private static Validation<Error, Dictionary<string, Dictionary<string, string>>> CreateParametersDictionnary(Domain.Entities.SvxlinkChannel channel, RadioProfil radioProfile)
        {
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
            return parameters;
        }

        private Validation<Error, LanguageExt.Unit> WriteDefaultSvxlinkConfig()
        {
            try
            {
                Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");
                File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", svxlinkManagerConfigRepository.GetDefaultSvxlinkConfig());

                return LanguageExt.Unit.Default;
            }
            catch (Exception)
            {
                return Error.New("Erreur lors de l'ecriture du fichier svxlink.conf.");
            }

            
        }

        protected virtual Validation<Error, LanguageExt.Unit> ReplaceSoundFile(ManagedChannel channel)
        {
            try
            {
                logger.LogInformation("Remplacement du fichier wav d'annonce.");

                if (!Directory.Exists("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager"))
                    Directory.CreateDirectory("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager");

                logger.LogInformation("Création du répertoire de son.");

                return LanguageExt.Unit.Default;
            }
            catch (Exception)
            {
                return Error.New("Erreur lors de la création du répertoire de son.");
            }

            

        }

    }


}
