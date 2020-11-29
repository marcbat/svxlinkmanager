using FileHelpers;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Pages.Wifi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public class WifiService
  {
    private readonly ILogger<WifiService> logger;

    public WifiService(ILogger<WifiService> logger)
    {
      this.logger = logger;
    }

    public List<Connection> GetConnections()
    {
      var connections = new List<Connection>();

      var (result, error) = ExecuteCommand("nmcli c");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return connections;
      }

      var engine = new FixedFileEngine<Connection>();
      connections = engine.ReadStringAsList(result);

      logger.LogInformation($"{connections.Count} connection trouvées.");

      return connections.Where(c => c.Type == "wifi")?.ToList();
    }

    public void Disconnect(string ssid)
    {
    }

    public void Connect(string ssid, string password)
    {
      ExecuteCommand($"nmcli d wifi connect {ssid} password {password}");
    }

    public bool IsConnectionExist(string name)
    {
      var connections = GetConnections();

      return connections.Any(c => c.Name == name);
    }

    public void AddConnection(Connection connection)
    {
      ExecuteCommand($"nmcli c add type wifi ssid {connection.Name} ifname wlan0 con-name {connection.Name}");

      ExecuteCommand($"nmcli c modify {connection.Name} wifi-sec.key-mgmt wpa-psk wifi-sec.psk {connection.Password}");
    }

    public List<Device> GetDevices()
    {
      var devices = new List<Device>();

      var (result, error) = ExecuteCommand("nmcli device wifi");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return devices;
      }

      logger.LogInformation($"Parsing du resultat.");
      logger.LogInformation(result);

      var engine = new FixedFileEngine<Device>();
      devices = engine.ReadStringAsList(result);

      logger.LogInformation($"{devices.Count} devices trouvées.");

      var connections = GetConnections();

      foreach (var device in devices)
        device.HasConnection = connections.Any(c => c.Name == device.Ssid);

      return devices;
    }

    private static (string, string) ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      var process = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      string error = process.StandardError.ReadToEnd();
      process.WaitForExit();

      return (result?.Trim(), error?.Trim());
    }
  }
}