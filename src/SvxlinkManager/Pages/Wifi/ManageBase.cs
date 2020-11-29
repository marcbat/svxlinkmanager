using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;
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
      base.OnInitialized();

      LoadDevices();
    }

    private void LoadDevices()
    {
      Devices = WifiService.GetDevices();
    }

    [Inject]
    public WifiService WifiService { get; set; }

    public List<Device> Devices { get; set; }

    public void Connect(string ssid)
    {
      InvokeAsync(() => StateHasChanged());
    }

    public void Up(string ssid)
    {
      InvokeAsync(() => StateHasChanged());
    }

    public void Disconnect(string ssid)
    {
      WifiService.Disconnect(ssid);
      InvokeAsync(() => StateHasChanged());
    }
  }
}