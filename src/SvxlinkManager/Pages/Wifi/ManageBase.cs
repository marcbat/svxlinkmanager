
using Microsoft.AspNetCore.Authorization;
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
  [Authorize]
  public class ManageBase : MediatrComponentBase
  {
    protected override void OnInitialized()
    {
      base.OnInitialized();

      WifiService.LoadDevices();
    }

    [Inject]
    public IWifiService WifiService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<Device> Devices => WifiService.Devices;

    public void Refresh()
    {
      WifiService.LoadDevices();
    }

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

    public void Disconnect(Device device)
    {
      Logger.LogInformation($"Suppression de la connection {device.Connection.Name} {device.Connection.Uuid}");

      WifiService.Disconnect(device.Connection);

    }
  }
}