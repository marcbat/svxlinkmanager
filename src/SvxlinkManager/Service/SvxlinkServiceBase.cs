using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

using System.IO;
using SvxlinkManager.Models;

namespace SvxlinkManager.Service
{
  public class SvxlinkServiceBase
  {
    private readonly ILogger<SvxlinkServiceBase> logger;
    private readonly TelemetryClient telemetry;
    private Process shell;
    private readonly Dictionary<int, Process> reflectorshells = new Dictionary<int, Process>();

    public SvxlinkServiceBase(ILogger<SvxlinkServiceBase> logger, TelemetryClient telemetry)
    {
      this.logger = logger;
      this.telemetry = telemetry;
    }

    /// <summary>
    /// Occurs when svxlink is connected
    /// </summary>
    public event Action<ChannelBase> Connected;

    /// <summary>
    /// Occurs when svxlink is disconnected
    /// </summary>
    public event Action Disconnected;

    /// <summary>
    /// Occurs when a new node join the channel
    /// </summary>
    public event Action<Node> NodeConnected;

    /// <summary>
    /// Occurs when a node quit the channel
    /// </summary>
    public event Action<Node> NodeDisconnected;

    /// <summary>
    /// Occurs when a node stop a transmission.
    /// </summary>
    public event Action<Node> NodeRx;

    /// <summary>
    /// Occurs when a node start a transmission.
    /// </summary>
    public event Action<Node> NodeTx;

    /// <summary>
    /// Occurs when an error is throw
    /// </summary>
    public event Action<string, string> Error;

    protected virtual void OnError(string title, string body) =>
      Error?.Invoke(title, body);

    /// <summary>
    /// List of connected nodes
    /// </summary>
    /// <value>connected nodes</value>
    public List<Node> Nodes { get; set; } = new List<Node>();

    /// <summary>
    /// Starts svxlink.
    /// </summary>
    /// <param name="reflector">The reflector.</param>
    /// <param name="runAsDaemon">Start the SvxReflector server as a daemon.</param>
    /// <param name="logFile">Specify a log file to put all output into. Both stdout and stderr will be redirected to the log file.</param>
    /// <param name="configFile">Specify which configuration file to use.</param>
    /// <param name="pidFile">Specify a pid file to write the process ID into.</param>
    /// <param name="runAs">Start SvxReflector as the specified user. The switch to the new user will happen after the log and pid files has been opened.</param>
    public virtual void StartSvxlink(ChannelBase channel, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null)
    {
      var parameters = new List<string>();

      if (runAsDaemon)
        parameters.Add("--daemon");

      if (logFile != null)
        parameters.Add($"--logfile={logFile}");

      if (configFile != null)
        parameters.Add($"--config={configFile}");

      if (pidFile != null)
        parameters.Add($"--pidfile={pidFile}");

      if (runAs != null)
        parameters.Add($"--runasuser={runAs}");

      shell = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"svxlink {string.Join(" ", parameters)}\"",
          RedirectStandardOutput = true,
          StandardOutputEncoding = Encoding.UTF8,
          RedirectStandardError = true,
          StandardErrorEncoding = Encoding.UTF8,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      shell.EnableRaisingEvents = true;
      shell.ErrorDataReceived += (s, e) =>
      {
        logger.LogInformation(e.Data);
        ParseLog(channel, e.Data);
      };
      shell.OutputDataReceived += (s, e) =>
      {
        logger.LogInformation(e.Data);
        ParseLog(channel, e.Data);
      };

      shell.Start();
      shell.BeginErrorReadLine();
      shell.BeginOutputReadLine();
    }

    /// <summary>
    /// Parse the Svxlink logs
    /// </summary>
    /// <param name="s">one Log line</param>
    public virtual void ParseLog(ChannelBase channel, string s)
    {
      if (string.IsNullOrEmpty(s))
        return;

      if (s.Contains("Connected nodes"))
      {
        Nodes.Clear();
        s.Split(':')[2].Split(',').ToList().ForEach(n => Nodes.Add(new Node { Name = n }));
        Connected?.Invoke(channel);
        return;
      }

      if (s.Contains("Node left"))
      {
        var node = new Node { Name = s.Split(":")[2] };
        Nodes.Remove(node);
        NodeDisconnected?.Invoke(node);
        return;
      }

      if (s.Contains("Node joined"))
      {
        var node = new Node { Name = s.Split(":")[2] };
        Nodes.Add(node);
        NodeConnected?.Invoke(node);
        return;
      }

      if (s.Contains("Talker start"))
      {
        var node = Nodes.Single(nx => nx.Equals(new Node { Name = s.Split(":")[2] }));
        node.ClassName = "node node-tx";
        NodeTx?.Invoke(node);
        return;
      }

      if (s.Contains("Talker stop"))
      {
        var node = Nodes.Single(nx => nx.Equals(new Node { Name = s.Split(":")[2] }));
        node.ClassName = "node";
        NodeRx?.Invoke(node);
        return;
      }

      if (s.Contains("Access denied"))
      {
        telemetry.TrackTrace($"Impossible de se connecter au salon {channel.Name}. <br/> Accès refusé.", SeverityLevel.Error, channel.TrackProperties);
        Error?.Invoke("Echec de la connexion.", $"Impossible de se connecter au salon {channel.Name}. <br/> Accès refusé.");
        return;
      }

      if (s.Contains("Host not found"))
      {
        telemetry.TrackTrace($"Impossible de se connecter au salon {channel.Name}. <br/> Server introuvable.", SeverityLevel.Error, channel.TrackProperties);
        Error?.Invoke("Echec de la connexion.", $"Impossible de se connecter au salon {channel.Name}. <br/> Server introuvable.");
        return;
      }
    }

    public virtual void StopSvxlink()
    {
      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");

      shell?.Dispose();

      Nodes.Clear();
      Disconnected?.Invoke();
    }

    /// <summary>
    /// Starts the reflector.
    /// </summary>
    /// <param name="reflector">The reflector.</param>
    /// <param name="runAsDaemon">Start the SvxReflector server as a daemon.</param>
    /// <param name="logFile">Specify a log file to put all output into. Both stdout and stderr will be redirected to the log file.</param>
    /// <param name="configFile">Specify which configuration file to use.</param>
    /// <param name="pidFile">Specify a pid file to write the process ID into.</param>
    /// <param name="runAs">Start SvxReflector as the specified user. The switch to the new user will happen after the log and pid files has been opened.</param>
    public virtual void StartReflector(Reflector reflector, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null)
    {
      var parameters = new List<string>();

      if (runAsDaemon)
        parameters.Add("--daemon");

      if (logFile != null)
        parameters.Add($"--logfile={logFile}");

      if (configFile != null)
        parameters.Add($"--config={configFile}");

      if (pidFile != null)
        parameters.Add($"--pidfile={pidFile}");

      if (runAs != null)
        parameters.Add($"--runasuser={runAs}");

      var reflectorshell = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"svxreflector {string.Join(" ", parameters)}\"",
          RedirectStandardOutput = true,
          StandardOutputEncoding = Encoding.UTF8,
          RedirectStandardError = true,
          StandardErrorEncoding = Encoding.UTF8,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      reflectorshell.EnableRaisingEvents = true;
      reflectorshell.ErrorDataReceived += (s, e) =>
      {
        logger.LogInformation(e.Data);
      };
      reflectorshell.OutputDataReceived += (s, e) =>
      {
        logger.LogInformation(e.Data);
      };

      reflectorshell.Start();
      reflectorshell.BeginErrorReadLine();
      reflectorshell.BeginOutputReadLine();

      reflectorshells.Add(reflector.Id, reflectorshell);
    }

    public virtual void StopReflector(Reflector reflector)
    {
      var pid = File.ReadAllText($"/var/run/reflector-{reflector.Id}.pid");

      if (pid != null)
        ExecuteCommand($"kill {pid}");

      if (!reflectorshells.ContainsKey(reflector.Id))
        return;

      reflectorshells[reflector.Id].Dispose();

      reflectorshells.Remove(reflector.Id);
    }

    /// <summary>
    /// Execute a cli command
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <returns>Console output</returns>
    protected virtual string ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      var process = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      process.WaitForExit();

      return result.Trim();
    }
  }
}