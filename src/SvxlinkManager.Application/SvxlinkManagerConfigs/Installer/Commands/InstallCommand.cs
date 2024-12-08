using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;

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

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{

    public record InstallCommand(Guid ConfigId,
                                 IEnumerable<Guid> InstallChannels,
                                 string CallSign,
                                 string AnnonceCallSign,
                                 Guid DefaultChannel,
                                 string Name,
                                 string RxFrequency,
                                 string TxFrequency,
                                 string Squelch,
                                 string TxCtcss,
                                 string RxCtCss,
                                 string Volume,
                                 string PreEmph,
                                 string HighPass,
                                 string LowPass,
                                 string SquelchDetection) : IRequest<Validation<Error,Guid>>;

    internal class InstallCommandHandler : IRequestHandler<InstallCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISa818Service sa818Service;
        private readonly ILogger<InstallCommandHandler> logger;

        public InstallCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISa818Service sa818Service, ILogger<InstallCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.sa818Service = sa818Service;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(InstallCommand request, CancellationToken cancellationToken)
        {
            
                logger.LogInformation("Execution du handler de la commande InstallCommand");

                var result = from install in InstallChannels(request.ConfigId, request.InstallChannels.ToList(), request.CallSign, request.AnnonceCallSign)
                             from _ in SetDefaultChannel(request.ConfigId, request.DefaultChannel)
                             from radioProfil in CreateRadioProfil(request.ConfigId, request.Name, request.RxFrequency, request.TxFrequency, request.Squelch, request.TxCtcss, request.RxCtCss, request.Volume, request.PreEmph, request.HighPass, request.LowPass, request.SquelchDetection)
                             from ___ in ApplyRadioProfile(request.ConfigId, radioProfil.Id)
                             select install;

               return Task.FromResult(result);
            
        }

        private Validation<Error, Guid> InstallChannels(Guid configId, List<Guid> installChannels, string callSign, string annonceCallSign)
        {
            try
            {
                logger.LogInformation("Début de l'installation des canaux.");
                var result = from config in svxlinkManagerConfigRepository.GetConfig(configId)
                             from originalChannels in svxlinkManagerConfigRepository.GetAllOriginalChannels()
                             from existingChannels in FilterExisting(originalChannels, installChannels)
                             from channels in AddDefaultCall(existingChannels, callSign, annonceCallSign)
                             from _ in UpdateChannelsInConfig(config, channels)
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Les canaux ont été installés avec succès.");

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de l'installation des canaux.");

                throw new Exception("Erreur lors de l'installation des canaux.", ex);
            }
        }

        private static Validation<Error, List<SvxlinkChannel>> FilterExisting(List<SvxlinkChannel> svxlinkChannels, IEnumerable<Guid> guids)
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

        private static Validation<Error, List<SvxlinkChannel>> AddDefaultCall(List<SvxlinkChannel> svxlinkChannels, string callSign, string annonceCallSign)
        {
            foreach (var channel in svxlinkChannels)
            {
                channel.SetCallSign(callSign);
                channel.SetReportCallSign(annonceCallSign);
            }
            return svxlinkChannels;
        }

        private static Validation<Error, LanguageExt.Unit> UpdateChannelsInConfig(SvxlinkManagerConfigAggregate config, List<SvxlinkChannel> channels)
        {
            foreach (var channel in channels)
            {
                config.AddSvxlinkChannel(channel);
            }
            return LanguageExt.Unit.Default;
        }

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

        private static Validation<Error, LanguageExt.Unit> SetAsDefault(SvxlinkChannel channel)
        {
            channel.IsDefault = true;
            channel.IsTemporized = false;

            return LanguageExt.Unit.Default;
        }

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
                         from _____ in WriteRadioProfil(radioProfile)
                         select radioProfile;

            return result;
        }

        private static Validation<Error, LanguageExt.Unit> EnableRadioProfile(RadioProfil radioProfil)
        {
            radioProfil.Enable = true;

            return LanguageExt.Unit.Default;
        }

        private Validation<Error, Guid> ApplyRadioProfile(Guid ConfigId, Guid RadioProfilId)
        {
            logger.LogInformation("Application du profil radio.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(ConfigId)
                         from radioProfile in config.GetRadioProfil(RadioProfilId)
                         from _ in WriteRadioProfil(radioProfile)
                         from __ in config.SetActiveRadioProfile(radioProfile.Id)
                         from ___ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select config.Id;

            return result;
        }

        private Validation<Error, LanguageExt.Unit> WriteRadioProfil(RadioProfil radioProfil)
        {
            if (radioProfil.HasSa818)
                return sa818Service.WriteRadioProfile(radioProfil);

            return LanguageExt.Unit.Default;
        }
    }
}
