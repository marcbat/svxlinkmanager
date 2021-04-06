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

    protected override void OnInitialized()
    {
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

    private List<Device> LoadDevices() => WifiService.GetDevices();

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
            Task.Run(async () =>
            {
              InstallerModel.Devices = LoadDevices();
              await InvokeAsync(() => StateHasChanged());
            });
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

    public void Install()
    {
      try
      {
        SeedUser();
        InstallChannels();
        SetDefaultChannel();
        CreateRadioProfile();
      }
      catch (Exception e)
      {
        Logger.LogError($"Erreur lors de l'intallation. {e.Message}");
        Telemetry.TrackException(new Exception("Erreur lors de l'installation", e));
      }
    }

    private void CreateRadioProfile()
    {
      try
      {
        if (InstallerModel.RadioProfile.HasSa818)
          Sa818Service.WriteRadioProfile(InstallerModel.RadioProfile);

        InstallerModel.RadioProfile.Enable = true;
        Repositories.RadioProfiles.Add(InstallerModel.RadioProfile);

        OnCreateRadioProfile?.Invoke();
      }
      catch (Exception e)
      {
        throw new Exception("Impossible de créer le profil radio", e);
      }
    }

    private void SetDefaultChannel()
    {
      try
      {
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

    private void InstallChannels()
    {
      try
      {
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

    private void SeedUser()
    {
      try
      {
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
  }
}