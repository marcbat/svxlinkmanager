using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  public class ManageBase : RepositoryComponentBase
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Wifi Page") { Url = new Uri("/Wifi/Manage", UriKind.Relative) });

      base.OnInitialized();

      LoadDevices();
    }

    private void LoadDevices()
    {
      Devices = WifiService.GetDevices();
    }

    [Inject]
    public IWifiService WifiService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<Device> Devices { get; set; }

    public void Connect(Device device)
    {
      Logger.LogInformation($"Creation de la connection {device.Ssid}");

      Telemetry.TrackEvent("Connect Wifi");

      WifiService.Connect(device);

      NavigationManager.NavigateTo("Wifi/Manage", true);
    }

    public void Up(Device device)
    {
      Logger.LogInformation($"Activation de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Up(device.Connection);

      Telemetry.TrackEvent("Activate Wifi");

      NavigationManager.NavigateTo("Wifi/Manage", true);
    }

    public void Down(Device device)
    {
      Logger.LogInformation($"Desactivation de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Down(device.Connection);

      Telemetry.TrackEvent("Deactivate Wifi");

      NavigationManager.NavigateTo("Wifi/Manage", true);
    }

    public void Disconnect(Device device)
    {
      Logger.LogInformation($"Suppression de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Disconnect(device.Connection);

      Telemetry.TrackEvent("Deconnect Wifi");

      NavigationManager.NavigateTo("Wifi/Manage", true);
    }
  }
}