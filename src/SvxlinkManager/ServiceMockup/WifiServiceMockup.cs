using Microsoft.Extensions.Logging;

using SvxlinkManager.Pages.Wifi;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.ServiceMockup
{
  public class WifiServiceMockup : IWifiService
  {
    private ILogger<WifiServiceMockup> logger;

    public WifiServiceMockup(ILogger<WifiServiceMockup> logger)
    {
      this.logger = logger;
    }

    public void Connect(Device device) => logger.LogInformation($"Connect {device.Ssid} device.");

    public void Disconnect(Connection connection) => logger.LogInformation($"Disconnect {connection.Name} connection.");

    public void Down(Connection connection) => logger.LogInformation($"Deactivate {connection.Name} connection.");

    public void Up(Connection connection) => logger.LogInformation($"Activate {connection.Name} connection.");

    public List<Device> GetDevices()
    {
      Thread.Sleep(4000);

      var devices = new List<Device>
      {
        new Device
        {
          InUse = "*",
          Ssid = "Maison Mockup",
          Mode = "Infra",
          Channel = "1",
          Rate = "130 Mbit/s",
          Signal = "100",
          Bars = "**__",
          Security = "WPA2",
          Connection = new Connection { Name = "Maison Mockup", Type = "wifi", Uuid = Guid.NewGuid().ToString() }
        },

        new Device
        {
          InUse = "",
          Ssid = "Honor 9 Mockup",
          Mode = "Infra",
          Channel = "1",
          Rate = "130 Mbit/s",
          Signal = "100",
          Bars = "***_",
          Security = "WPA2",
          Connection = new Connection { Name = "Honor 9 Mockup", Type = "wifi", Uuid = Guid.NewGuid().ToString() }
        },

        new Device
        {
          InUse = "",
          Ssid = "HotSpot Mockup",
          Mode = "Infra",
          Channel = "1",
          Rate = "130 Mbit/s",
          Signal = "100",
          Bars = "***_",
          Security = "WPA2"
        }
      };

      return devices;
    }
  }
}