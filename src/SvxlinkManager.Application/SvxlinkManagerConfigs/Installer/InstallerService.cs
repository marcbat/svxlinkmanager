using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer
{
    public class InstallerService
    {
        private readonly ChannelService channelService;
        private readonly IAuthentificationService authentificationService;
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISa818Service sa818Service;
        private readonly ILogger<InstallerService> logger;

        public InstallerService(ChannelService channelService, IAuthentificationService authentificationService, ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISa818Service sa818Service, ILogger<InstallerService> logger)
        {
            this.channelService = channelService;
            this.authentificationService = authentificationService;
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.sa818Service = sa818Service;
            this.logger = logger;
        }

        
        ///<summary>
        /// Installe le gestionnaire Svxlink avec les paramètres spécifiés.
        /// </summary>
        /// <param name="userName">Le nom d'utilisateur pour l'authentification.</param>
        /// <param name="password">Le mot de passe pour l'authentification.</param>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="installChannels">Les identifiants des canaux à installer.</param>
        /// <param name="callSign">L'indicatif d'appel.</param>
        /// <param name="annonceCallSign">L'indicatif d'appel d'annonce.</param>
        /// <param name="defaultChannelId">L'identifiant du canal par défaut.</param>
        /// <param name="name">Le nom du profil radio.</param>
        /// <param name="rxFrequency">La fréquence de réception.</param>
        /// <param name="txFrequency">La fréquence de transmission.</param>
        /// <param name="squelch">Le squelch.</param>
        /// <param name="txCtcss">Le CTCSS de transmission.</param>
        /// <param name="rxCtCss">Le CTCSS de réception.</param>
        /// <param name="volume">Le volume.</param>
        /// <param name="preEmph">La pré-accentuation.</param>
        /// <param name="highPass">Le filtre passe-haut.</param>
        /// <param name="lowPass">Le filtre passe-bas.</param>
        /// <param name="squelchDetection">La détection de squelch.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        internal Validation<Error, Guid> InstallSvxlinkManager(
                                 string userName,
                                 string password,
                                 Guid configId,
                                 IEnumerable<Guid> installChannels,
                                 string callSign,
                                 string annonceCallSign,
                                 Guid defaultChannelId,
                                 string name,
                                 string rxFrequency,
                                 string txFrequency,
                                 string squelch,
                                 string txCtcss,
                                 string rxCtCss,
                                 string volume,
                                 string preEmph,
                                 string highPass,
                                 string lowPass,
                                 string squelchDetection)
        {
            var result = from userGuid in authentificationService.SeedUser(userName, password)
                         from _ in CreateEmptyConfiguration(configId)
                         from __ in SetupConfiguration(configId, installChannels, callSign, annonceCallSign, defaultChannelId, name, rxFrequency, txFrequency, squelch, txCtcss, rxCtCss, volume, preEmph, highPass, lowPass, squelchDetection)
                         from ___ in channelService.StartDefaultChannel(configId)
                         select configId;

            return result;
        }

        /// <summary>
        /// Crée une configuration vide avec l'identifiant spécifié.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        private Validation<Error, Guid> CreateEmptyConfiguration(Guid configId)
        {
            var result = from config in svxlinkManagerConfigRepository.FindConfig(configId)
                         where config.IsNone
                         from newConfig in SvxlinkManagerConfigAggregate.Create(configId)
                         from id in svxlinkManagerConfigRepository.Create(newConfig)
                         select id;

            return result;
        }

        /// <summary>
        /// Configure la configuration avec les paramètres spécifiés.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="installChannels">Les identifiants des canaux à installer.</param>
        /// <param name="callSign">L'indicatif d'appel.</param>
        /// <param name="annonceCallSign">L'indicatif d'appel d'annonce.</param>
        /// <param name="defaultChannelId">L'identifiant du canal par défaut.</param>
        /// <param name="name">Le nom du profil radio.</param>
        /// <param name="rxFrequency">La fréquence de réception.</param>
        /// <param name="txFrequency">La fréquence de transmission.</param>
        /// <param name="squelch">Le squelch.</param>
        /// <param name="txCtcss">Le CTCSS de transmission.</param>
        /// <param name="rxCtCss">Le CTCSS de réception.</param>
        /// <param name="volume">Le volume.</param>
        /// <param name="preEmph">La pré-accentuation.</param>
        /// <param name="highPass">Le filtre passe-haut.</param>
        /// <param name="lowPass">Le filtre passe-bas.</param>
        /// <param name="squelchDetection">La détection de squelch.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        private Validation<Error, Guid> SetupConfiguration(Guid configId,
                                 IEnumerable<Guid> installChannels,
                                 string callSign,
                                 string annonceCallSign,
                                 Guid defaultChannelId,
                                 string name,
                                 string rxFrequency,
                                 string txFrequency,
                                 string squelch,
                                 string txCtcss,
                                 string rxCtCss,
                                 string volume,
                                 string preEmph,
                                 string highPass,
                                 string lowPass,
                                 string squelchDetection)
        {
            var result = from install in InstallChannels(configId, installChannels.ToList(), callSign, annonceCallSign)
                         from _ in SetDefaultChannel(configId, defaultChannelId)
                         from radioProfil in CreateRadioProfil(configId, name, rxFrequency, txFrequency, squelch, txCtcss, rxCtCss, volume, preEmph, highPass, lowPass, squelchDetection)
                         from ___ in ApplyRadioProfile(configId, radioProfil.Id)
                         select install;

            return result;
        }

        /// <summary>
        /// Installe les canaux spécifiés dans la configuration.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="installChannels">La liste des identifiants des canaux à installer.</param>
        /// <param name="callSign">L'indicatif d'appel.</param>
        /// <param name="annonceCallSign">L'indicatif d'appel d'annonce.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        private Validation<Error, Guid> InstallChannels(Guid configId, List<Guid> installChannels, string callSign, string annonceCallSign)
        {

            logger.LogInformation("Début de l'installation des canaux.");
            var result = from config in svxlinkManagerConfigRepository.GetConfig(configId)
                         from originalChannels in svxlinkManagerConfigRepository.GetAllOriginalChannels()
                         from existingChannels in FilterChannelsToKeep(originalChannels, installChannels)
                         from channels in AddCallSignsToChannels(existingChannels, callSign, annonceCallSign)
                         from _ in UpdateChannelsInConfig(config, channels)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Les canaux ont été installés avec succès.");

            return result;


        }

        /// <summary>
        /// Filtre les canaux à conserver en fonction des identifiants spécifiés.
        /// </summary>
        /// <param name="svxlinkChannels">La liste des canaux Svxlink disponibles.</param>
        /// <param name="guids">Les identifiants des canaux à conserver.</param>
        /// <returns>Une liste des canaux filtrés ou une erreur.</returns>
        private static Validation<Error, List<SvxlinkChannel>> FilterChannelsToKeep(List<SvxlinkChannel> svxlinkChannels, IEnumerable<Guid> guids)
        {
            var existingChannels = new List<SvxlinkChannel>();

            foreach (var guid in guids)
            {
                var channel = svxlinkChannels.FirstOrDefault(c => c.Id == guid);
                if (channel is not null)
                {
                    existingChannels.Add(channel);
                }
            }

            return existingChannels;
        }

        /// <summary>
        /// Ajoute les indicatifs d'appel aux canaux spécifiés.
        /// </summary>
        /// <param name="svxlinkChannels">La liste des canaux Svxlink.</param>
        /// <param name="callSign">L'indicatif d'appel.</param>
        /// <param name="annonceCallSign">L'indicatif d'appel d'annonce.</param>
        /// <returns>Une liste des canaux avec les indicatifs d'appel mis à jour ou une erreur.</returns>
        private static Validation<Error, List<SvxlinkChannel>> AddCallSignsToChannels(List<SvxlinkChannel> svxlinkChannels, string callSign, string annonceCallSign)
        {
            foreach (var channel in svxlinkChannels)
            {
                channel.SetCallSign(callSign);
                channel.SetReportCallSign(annonceCallSign);
            }
            return svxlinkChannels;
        }

        /// <summary>
        /// Met à jour les canaux dans la configuration spécifiée.
        /// </summary>
        /// <param name="config">L'agrégat de configuration du gestionnaire Svxlink.</param>
        /// <param name="channels">La liste des canaux à ajouter à la configuration.</param>
        /// <returns>Un objet Validation contenant Unit ou une erreur.</returns>
        private static Validation<Error, Unit> UpdateChannelsInConfig(SvxlinkManagerConfigAggregate config, List<SvxlinkChannel> channels)
        {
            foreach (var channel in channels)
            {
                config.AddSvxlinkChannel(channel);
            }
            return Unit.Default;
        }

        /// <summary>
        /// Configure le canal par défaut avec l'identifiant spécifié.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="channelId">L'identifiant du canal à définir comme canal par défaut.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        private Validation<Error, Guid> SetDefaultChannel(Guid configId, Guid channelId)
        {
            logger.LogInformation("Début de la configuration du channel par defaut.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(configId)
                         from channel in config.GetSvxlinkChannel(channelId)
                         from _ in SetAsDefault(channel)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            logger.LogInformation("Le channel par defaut a été configuré.");

            return result;
        }

        /// <summary>
        /// Définit le canal comme canal par défaut.
        /// </summary>
        /// <param name="channel">Le canal à définir comme canal par défaut.</param>
        /// <returns>Un objet Validation contenant Unit ou une erreur.</returns>
        private static Validation<Error, Unit> SetAsDefault(SvxlinkChannel channel)
        {
            channel.IsDefault = true;
            channel.IsTemporized = false;

            return Unit.Default;
        }

        /// <summary>
        /// Crée un nouveau profil radio avec les paramètres spécifiés.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="name">Le nom du profil radio.</param>
        /// <param name="rxFrequency">La fréquence de réception.</param>
        /// <param name="txFrequency">La fréquence de transmission.</param>
        /// <param name="squelch">Le squelch.</param>
        /// <param name="txCtcss">Le CTCSS de transmission.</param>
        /// <param name="rxCtCss">Le CTCSS de réception.</param>
        /// <param name="volume">Le volume.</param>
        /// <param name="preEmph">La pré-accentuation.</param>
        /// <param name="highPass">Le filtre passe-haut.</param>
        /// <param name="lowPass">Le filtre passe-bas.</param>
        /// <param name="squelchDetection">La détection de squelch.</param>
        /// <returns>Un objet Validation contenant le profil radio ou une erreur.</returns>
        private Validation<Error, RadioProfil> CreateRadioProfil(Guid configId, string name,
                               string rxFrequency,
                               string txFrequency,
                               string squelch,
                               string txCtcss,
                               string rxCtCss,
                               string volume,
                               string preEmph,
                               string highPass,
                               string lowPass,
                               string squelchDetection)
        {
            logger.LogInformation("Début de la création d'un nouveau profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(configId)
                         from _ in config.DeleteAllRadioProfils()
                         from radioProfile in RadioProfil.Create(Guid.NewGuid(), name, rxFrequency, txFrequency, squelch, txCtcss, rxCtCss, volume, preEmph, highPass, lowPass, squelchDetection)
                         from __ in EnableRadioProfile(radioProfile)
                         from ___ in config.AddRadioProfil(radioProfile)
                         from ____ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select radioProfile;

            return result;
        }

        /// <summary>
        /// Active le profil radio spécifié.
        /// </summary>
        /// <param name="radioProfil">Le profil radio à activer.</param>
        /// <returns>Un objet Validation contenant Unit ou une erreur.</returns>
        private static Validation<Error, Unit> EnableRadioProfile(RadioProfil radioProfil)
        {
            radioProfil.Enable = true;

            return Unit.Default;
        }

        /// <summary>
        /// Applique le profil radio spécifié à la configuration.
        /// </summary>
        /// <param name="configId">L'identifiant de la configuration.</param>
        /// <param name="radioProfilId">L'identifiant du profil radio à appliquer.</param>
        /// <returns>Un objet Validation contenant l'identifiant de la configuration ou une erreur.</returns>
        private Validation<Error, Guid> ApplyRadioProfile(Guid configId, Guid radioProfilId)
        {
            logger.LogInformation("Application du profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(configId)
                         from radioProfile in config.GetRadioProfil(radioProfilId)
                         from _ in WriteRadioProfilInSa818(radioProfile)
                         from __ in config.SetActiveRadioProfile(radioProfile.Id)
                         from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            return result;
        }

        /// <summary>
        /// Écrit le profil radio dans le SA818 si applicable.
        /// </summary>
        /// <param name="radioProfil">Le profil radio à écrire.</param>
        /// <returns>Un objet Validation contenant Unit ou une erreur.</returns>
        private Validation<Error, Unit> WriteRadioProfilInSa818(RadioProfil radioProfil)
        {
            if (radioProfil.HasSa818)
                return sa818Service.WriteRadioProfile(radioProfil);

            return Unit.Default;
        }
    }
}
