using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Pages.Wifi;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
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
    Update
  }

  public class HomeBase : RepositoryComponentBase
  {
    private InstallationStatus installationStatus = InstallationStatus.Security;

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

    private List<Device> LoadDevices() => WifiService.GetDevices();

    private List<SvxlinkChannel> LoadChannels() => Repositories.SvxlinkChannels.GetAll().ToList();

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
            break;

          default:

            break;
        }

        installationStatus = value;
      }
    }
  }
}