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
  public interface IWifiService
  {
    void Connect(Device device);

    void Disconnect(Connection connection);

    void Down(Connection connection);

    List<Device> GetDevices();

    void Up(Connection connection);
  }

  public class WifiService : IWifiService
  {
    private readonly ILogger<WifiService> logger;

    public WifiService(ILogger<WifiService> logger)
    {
      this.logger = logger;
    }

    /// <summary>Create a new connection for the device</summary>
    /// <param name="device">The device.</param>
    public void Connect(Device device)
    {
      var (result, error) = ExecuteCommand($"nmcli d wifi connect \"{device.Ssid}\" password {device.Password}");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);
    }

    /// <summary>Remove the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Disconnect(Connection connection)
    {
      var (result, error) = ExecuteCommand($"nmcli connection delete {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);
    }

    /// <summary>Activate the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Up(Connection connection)
    {
      var (result, error) = ExecuteCommand($"nmcli connection up {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);
    }

    /// <summary>Deactivate the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Down(Connection connection)
    {
      var (result, error) = ExecuteCommand($"nmcli connection down {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);
    }

    /// <summary>Get detected wifi devices</summary>
    /// <returns>List of detected devices</returns>
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
        device.Connection = connections.FirstOrDefault(c => c.Name == device.Ssid);

      return devices;
    }

    private List<Connection> GetConnections()
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

    private (string, string) ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      logger.LogInformation($"Execution de la commande {cmd}.");

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