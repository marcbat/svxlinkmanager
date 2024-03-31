using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Pages.Updater;
using SvxlinkManager.Pages.Wifi;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
    public enum InstallationStatus
    {
        Security,
        Channel,
        DefaultChannel,
        RadioProfile,
        Wifi,
        Update,
        Resume,
        Progress
    }

    public class HomeBase : MediatrComponentBase
    {
        private InstallationStatus installationStatus = InstallationStatus.Security;

        public event Action OnSetUser;

        public event Action OnInstallChannels;

        public event Action OnSetDefaultChannel;

        public event Action OnCreateRadioProfile;

        public event Action OnDownloadStart;

        public event Action<int> OnDownloadProgress;

        public event Action OnDownloadComplete;

        public event Action OnInstall;

        protected override async void OnInitialized()
        {
            if (UserManager.Users.Any())
                NavigationManager.NavigateTo("Identity/Account/Login", true);

            base.OnInitialized();

            InstallerModel = new InstallerModel
            {
                Channels = await LoadChannelsAsync()
            };
        }

        [Inject]
        public IWifiService WifiService { get; set; }

        [Inject]
        public UpdaterService UpdaterService { get; set; }

        [Inject]
        public UserManager<IdentityUser> UserManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISvxlinkServiceBase SvxLinkService { get; set; }

        private async Task<List<SvxlinkChannel>> LoadChannelsAsync()
        {
            var channels = await Mediatr.Send(new GetAllSvxlinChannelQuery(Guid.NewGuid()));

            return channels.Select<Domain.Entities.SvxlinkChannel, SvxlinkChannel>(c=> c).ToList();
        }

        private Release LoadLastRelease() => UpdaterService.GetLastRelease();

        public bool IsCurrentRelease() => UpdaterService.IsCurrent(InstallerModel.LastRelease);

        public InstallerModel InstallerModel { get; set; }

        public void Connect(Device device)
        {
            Logger.LogInformation($"Creation de la connection {device.Ssid}");

            WifiService.Connect(device);
        }

        public void Up(Device device)
        {
            Logger.LogInformation($"Activation de la connection {device.Connection.Name} {device.Connection.Uuid}");

            WifiService.Up(device.Connection);
        }

        public void Down(Device device)
        {
            Logger.LogInformation($"Desactivation de la connection {device.Connection.Name} {device.Connection.Uuid}");

            WifiService.Down(device.Connection);
        }

        public string InformationalVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public InstallationStatus InstallationStatus
        {
            get => installationStatus;
            set
            {
                switch (value)
                {
                    case InstallationStatus.Security:
                    case InstallationStatus.Channel:
                    case InstallationStatus.DefaultChannel:
                    case InstallationStatus.RadioProfile:
                        break;

                    case InstallationStatus.Wifi:
                        WifiService.LoadDevices();
                        InstallerModel.Devices = WifiService.Devices;
                        break;

                    case InstallationStatus.Update:
                        InstallerModel.LastRelease = LoadLastRelease();
                        InstallerModel.CurrentVersion = UpdaterService.CurrentVersion;
                        break;

                    default:

                        break;
                }

                installationStatus = value;
            }
        }

        /// <summary>Installation de SvxlinkManager</summary>
        public async Task InstallAsync()
        {
            try
            {
                Logger.LogInformation("Installation du profil radio.");
                InstallerModel.TrackProperties.ToList().ForEach(x => Logger.LogInformation($"{x.Key}: {x.Value}"));

                SeedUser();
                await InstallChannelsAsync();
                await SetDefaultChannelAsync();
                await CreateRadioProfileAsync();
                if (InstallerModel.UpdateToLastRelease)
                    Update();
                else
                {
                    await Mediatr.Send(new StartDefaultChannelCommand(Guid.NewGuid()));
                    NavigationManager.NavigateTo("Identity/Account/Login", true);
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Erreur lors de l'intallation. {e.Message}");
            }
        }

        /// <summary>Crée le profil radio et programme le SA818 si necessaire</summary>
        /// <exception cref="Exception">Impossible de créer le profil radio</exception>
        private async Task CreateRadioProfileAsync()
        {
            try
            {
                Logger.LogInformation("Installation du profil radio.");

                await Mediatr.Send(new CreateRadioProfilCommand(
                    Guid.NewGuid(),
                    InstallerModel.RadioProfile.Name,
                    InstallerModel.RadioProfile.RxFequ,
                    InstallerModel.RadioProfile.TxFrequ,
                    InstallerModel.RadioProfile.Squelch,
                    InstallerModel.RadioProfile.TxCtcss,
                    InstallerModel.RadioProfile.RxCtcss,
                    InstallerModel.RadioProfile.Volume,
                    InstallerModel.RadioProfile.PreEmph,
                    InstallerModel.RadioProfile.HightPass,
                    InstallerModel.RadioProfile.LowPass,
                    InstallerModel.RadioProfile.SquelchDetection
                ));

                OnCreateRadioProfile?.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception("Impossible de créer le profil radio", e);
            }
        }

        /// <summary>Définition du salon par défaut</summary>
        /// <exception cref="Exception">Impossible de définir le salon par défaut.</exception>
        private async Task SetDefaultChannelAsync()
        {
            try
            {
                Logger.LogInformation("Configuration du salon par défaut.");

                await Mediatr.Send(new SetDefaultChannelCommand(Guid.NewGuid(), InstallerModel.DefaultChannel.Id));

                OnSetDefaultChannel?.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception("Impossible de définir le salon par défaut.", e);
            }
        }

        /// <summary>Installe les salons</summary>
        /// <exception cref="Exception">Impossible de définir les salons à installer</exception>
        private async Task InstallChannelsAsync()
        {
            try
            {
                Logger.LogInformation("Installation des salons.");

                await Mediatr.Send(new InstallChannelsCommand(Guid.NewGuid(), InstallerModel.ChannelsToDelete.Select(c => c.Id), InstallerModel.CallSign, InstallerModel.AnnonceCallSign));

                OnInstallChannels?.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception("Impossible de définir les salons à installer", e);
            }
        }

        /// <summary>Ajout l'utilisateur admin</summary>
        /// <exception cref="Exception">Impossible de créer l'utilisateur par défaut.</exception>
        private void SeedUser()
        {
            try
            {
                Logger.LogInformation("Installation de l'utilisateur par défaut.");

                var user = new IdentityUser
                {
                    UserName = InstallerModel.UserName,
                    Email = InstallerModel.UserName
                };

                var result = UserManager.CreateAsync(user, InstallerModel.Password).Result;

                if (result.Succeeded)
                    UserManager.AddToRoleAsync(user, "Admin").Wait();

                OnSetUser?.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception("Impossible de créer l'utilisateur par défaut.", e);
            }
        }

        /// <summary>Telechargement de la mise à jour</summary>
        private void Update()
        {
            Logger.LogInformation("Telechargement de la mise à jour.");

            UpdaterService.OnDownloadStart += r => OnDownloadStart?.Invoke();
            UpdaterService.OnDownloadProgress += x => OnDownloadProgress?.Invoke(x.progressPercentage);
            UpdaterService.OndownloadComplete += r =>
            {
                OnDownloadComplete?.Invoke();
                InstallUpdate();
            };

            UpdaterService.Download(InstallerModel.LastRelease);
        }

        /// <summary>Installation de la mise à jour</summary>
        private void InstallUpdate()
        {
            Logger.LogInformation("Installation de la mise à jour et redemarrage. ");

            OnInstall?.Invoke();
            UpdaterService.Install(InstallerModel.LastRelease);
        }
    }
}