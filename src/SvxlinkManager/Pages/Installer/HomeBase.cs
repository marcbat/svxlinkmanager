using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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

  public class HomeBase : RepositoryComponentBase
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

    protected override void OnInitialized()
    {
      if (UserManager.Users.Any())
        NavigationManager.NavigateTo("Identity/Account/Login", true);

      base.OnInitialized();

      InstallerModel = new InstallerModel
      {
        Channels = LoadChannels()
      };
    }

    [Inject]
    public IWifiService WifiService { get; set; }

    [Inject]
    public UpdaterService UpdaterService { get; set; }

    [Inject]
    public ISa818Service Sa818Service { get; set; }

    [Inject]
    public UserManager<IdentityUser> UserManager { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    private List<SvxlinkChannel> LoadChannels() => Repositories.SvxlinkChannels.GetAll().ToList();

    private Release LoadLastRelease() => UpdaterService.GetLastRelease();

    public bool IsCurrentRelease() => UpdaterService.IsCurrent(InstallerModel.LastRelease);

    public InstallerModel InstallerModel { get; set; }

    public void Connect(Device device)
    {
      Logger.LogInformation($"Creation de la connection {device.Ssid}");

      Telemetry.TrackEvent("Connect Wifi");

      WifiService.Connect(device);
    }

    public void Up(Device device)
    {
      Logger.LogInformation($"Activation de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Up(device.Connection);

      Telemetry.TrackEvent("Activate Wifi");
    }

    public void Down(Device device)
    {
      Logger.LogInformation($"Desactivation de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Down(device.Connection);

      Telemetry.TrackEvent("Deactivate Wifi");
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
    public void Install()
    {
      try
      {
        Logger.LogInformation("Installation du profil radio.");
        InstallerModel.TrackProperties.ToList().ForEach(x => Logger.LogInformation($"{x.Key}: {x.Value}"));

        Telemetry.TrackEvent("Installation de SvxlinkMananger", InstallerModel.TrackProperties);

        SeedUser();
        InstallChannels();
        SetDefaultChannel();
        CreateRadioProfile();
        if (InstallerModel.UpdateToLastRelease)
          Update();
        else
        {
          SvxLinkService.StartDefaultChannel();
          NavigationManager.NavigateTo("Identity/Account/Login", true);
        }
      }
      catch (Exception e)
      {
        Logger.LogError($"Erreur lors de l'intallation. {e.Message}");
        Telemetry.TrackException(new Exception("Erreur lors de l'installation", e));
      }
    }

    /// <summary>Crée le profil radio et programme le SA818 si necessaire</summary>
    /// <exception cref="Exception">Impossible de créer le profil radio</exception>
    private void CreateRadioProfile()
    {
      try
      {
        Logger.LogInformation("Installation du profil radio.");
        Telemetry.TrackEvent("Installation du profil radio.");

        if (InstallerModel.RadioProfile.HasSa818)
          Sa818Service.WriteRadioProfile(InstallerModel.RadioProfile);

        foreach (var radioprofile in Repositories.RadioProfiles.GetAll())
          Repositories.Repository<Models.RadioProfile>().Delete(radioprofile.Id);

        InstallerModel.RadioProfile.Enable = true;
        Repositories.RadioProfiles.Add(InstallerModel.RadioProfile);

        OnCreateRadioProfile?.Invoke();
      }
      catch (Exception e)
      {
        throw new Exception("Impossible de créer le profil radio", e);
      }
    }

    /// <summary>Définition du salon par défaut</summary>
    /// <exception cref="Exception">Impossible de définir le salon par défaut.</exception>
    private void SetDefaultChannel()
    {
      try
      {
        Logger.LogInformation("Configuration du salon par défaut.");
        Telemetry.TrackEvent("Configuration du salon par défaut.");

        var channel = Repositories.Channels.Get(InstallerModel.DefaultChannel.Id);

        channel.IsDefault = true;
        channel.IsTemporized = false;
        Repositories.Channels.Update(channel);

        OnSetDefaultChannel?.Invoke();
      }
      catch (Exception e)
      {
        throw new Exception("Impossible de définir le salon par défaut.", e);
      }
    }

    /// <summary>Installe les salons</summary>
    /// <exception cref="Exception">Impossible de définir les salons à installer</exception>
    private void InstallChannels()
    {
      try
      {
        Logger.LogInformation("Installation des salons.");
        Telemetry.TrackEvent("Installation des salons.");

        foreach (var channel in InstallerModel.ChannelsToDelete)
          Repositories.Channels.Delete(channel.Id);

        foreach (var channel in Repositories.SvxlinkChannels.GetAll())
        {
          channel.CallSign = InstallerModel.CallSign;
          channel.ReportCallSign = InstallerModel.AnnonceCallSign;

          Repositories.Channels.Update(channel);
        }

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
        Telemetry.TrackEvent("Installation de l'utilisateur par défaut.");

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
      Telemetry.TrackEvent("Telechargement de la mise à jour.");

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
      Telemetry.TrackEvent("Installation de la mise à jour.");

      OnInstall?.Invoke();
      UpdaterService.Install(InstallerModel.LastRelease);
    }
  }
}