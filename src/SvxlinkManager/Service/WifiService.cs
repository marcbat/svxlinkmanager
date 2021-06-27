using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Exceptions;
using SvxlinkManager.Pages.Wifi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public interface IWifiService
  {
    List<Device> Devices { get; }

    void LoadDevices();

    void Connect(Device device);

    void Disconnect(Connection connection);

    void Down(Connection connection);

    void Up(Connection connection);
  }

  public class WifiService : IWifiService
  {
    private readonly ILogger<WifiService> logger;
    private readonly TelemetryClient telemetry;

    public List<Device> Devices { get; } = new List<Device>();

    public WifiService(ILogger<WifiService> logger, TelemetryClient telemetry)
    {
      this.logger = logger;
      this.telemetry = telemetry;
    }

    /// <summary>Create a new connection for the device</summary>
    /// <param name="device">The device.</param>
    public void Connect(Device device)
    {
      telemetry.TrackEvent("Connect Wifi", new Dictionary<string, string> { { nameof(device.Ssid), device.Ssid } });

      var (result, error) = ExecuteCommand($"nmcli d wifi connect \"{device.Ssid}\" password {device.Password}");

      if (!string.IsNullOrEmpty(error))
      {
        telemetry.TrackException(new WifiException(error));
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);

      LoadDevices();
    }

    /// <summary>Remove the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Disconnect(Connection connection)
    {
      telemetry.TrackEvent("Disconnect Wifi", new Dictionary<string, string> { { nameof(connection.Name), connection.Name } });

      var (result, error) = ExecuteCommand($"nmcli connection delete {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        telemetry.TrackException(new WifiException(error));
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);

      LoadDevices();
    }

    /// <summary>Activate the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Up(Connection connection)
    {
      telemetry.TrackEvent("Active Wifi connection", new Dictionary<string, string> { { nameof(connection.Name), connection.Name } });

      var (result, error) = ExecuteCommand($"nmcli connection up {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        telemetry.TrackException(new WifiException(error));
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);

      LoadDevices();
    }

    /// <summary>Deactivate the connection</summary>
    /// <param name="connection">The connection.</param>
    public void Down(Connection connection)
    {
      telemetry.TrackEvent("Deactivate Wifi connection", new Dictionary<string, string> { { nameof(connection.Name), connection.Name } });

      var (result, error) = ExecuteCommand($"nmcli connection down {connection.Uuid}");

      if (!string.IsNullOrEmpty(error))
      {
        telemetry.TrackException(new WifiException(error));
        logger.LogError(error);
        return;
      }

      logger.LogInformation(result);

      LoadDevices();
    }

    /// <summary>Get detected wifi devices</summary>
    /// <returns>List of detected devices</returns>
    public void LoadDevices()
    {
      var devices = new List<Device>();

      try
      {
        var (result, error) = ExecuteCommand("nmcli -f IN-USE,SSID,MODE,CHAN,RATE,SIGNAL,BARS,SECURITY device wifi");

        if (!string.IsNullOrEmpty(error))
        {
          logger.LogError(error);
          throw new Exception(error);
        }

        devices = ParseDeviceConsoleOutput(result);

        logger.LogInformation($"{devices.Count} devices trouvées.");

        var connections = GetConnections();

        foreach (var device in devices)
          device.Connection = connections.FirstOrDefault(c => c.Name == device.Ssid);
      }
      catch (Exception e)
      {
        telemetry.TrackException(new WifiException("Echec de la liste des devices", e));
      }

      Devices.Clear();

      foreach (var d in devices)
        Devices.Add(d);
    }

    /// <summary>Extract list of wifi devices from console output</summary>
    /// <param name="output">Consoel output</param>
    /// <returns>List of wifi devices</returns>
    public List<Device> ParseDeviceConsoleOutput(string output)
    {
      var devices = new List<Device>();

      using var reader = new StringReader(output);
      string first = reader.ReadLine();

      List<(int, int)> indexes = new List<(int, int)>
      {
        (0, output.IndexOf("SSID") - 1),
        (output.IndexOf("SSID"), output.IndexOf("MODE") - output.IndexOf("SSID")),
        (output.IndexOf("MODE"), output.IndexOf("CHAN") - output.IndexOf("MODE")),
        (output.IndexOf("CHAN"), output.IndexOf("RATE") - output.IndexOf("CHAN")),
        (output.IndexOf("RATE"), output.IndexOf("SIGNAL") - output.IndexOf("RATE")),
        (output.IndexOf("SIGNAL"), output.IndexOf("BARS") - output.IndexOf("SIGNAL")),
        (output.IndexOf("BARS"), output.IndexOf("SECURITY") - output.IndexOf("BARS")),
        (output.IndexOf("SECURITY"), 0)
      };

      string line;
      while ((line = reader.ReadLine()) != null)
      {
        logger.LogInformation($"Parsing de la ligne device {line}");

        var device = new Device
        {
          InUse = line.Substring(indexes[0].Item1, indexes[0].Item2)?.Trim(),
          Ssid = line.Substring(indexes[1].Item1, indexes[1].Item2)?.Trim(),
          Mode = line.Substring(indexes[2].Item1, indexes[2].Item2)?.Trim(),
          Channel = line.Substring(indexes[3].Item1, indexes[3].Item2)?.Trim(),
          Rate = line.Substring(indexes[4].Item1, indexes[4].Item2)?.Trim(),
          Signal = line.Substring(indexes[5].Item1, indexes[5].Item2)?.Trim(),
          Bars = line.Substring(indexes[6].Item1, indexes[6].Item2)?.Trim(),
          Security = line.Substring(indexes[7].Item1)?.Trim()
        };

        devices.Add(device);
      }

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

      connections = ParseConnectionConsoleOutput(result);

      logger.LogInformation($"{connections.Count} connection trouvées.");

      return connections.Where(c => c.Type == "wifi")?.ToList();
    }

    public List<Connection> ParseConnectionConsoleOutput(string output)
    {
      var connections = new List<Connection>();

      using var reader = new StringReader(output);
      string first = reader.ReadLine();

      List<(int, int)> indexes = new List<(int, int)>
      {
        (0, output.IndexOf("UUID") - 1),
        (output.IndexOf("UUID"), output.IndexOf("TYPE") - output.IndexOf("UUID")),
        (output.IndexOf("TYPE"), output.IndexOf("DEVICE") - output.IndexOf("TYPE")),
        (output.IndexOf("DEVICE"), 0)
      };

      string line;
      while ((line = reader.ReadLine()) != null)
      {
        logger.LogInformation($"Parsing de la ligne connection {line}");

        var connection = new Connection
        {
          Name = line.Substring(indexes[0].Item1, indexes[0].Item2)?.Trim(),
          Uuid = line.Substring(indexes[1].Item1, indexes[1].Item2)?.Trim(),
          Type = line.Substring(indexes[2].Item1, indexes[2].Item2)?.Trim(),
          Device = line.Substring(indexes[3].Item1, indexes[3].Item2)?.Trim()
        };

        connections.Add(connection);
      }

      return connections;
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