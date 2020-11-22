using IniParser;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Repositories;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace SvxlinkManager.Service
{
  public class SvxLinkService
  {
    public event Action Connected;
    public event Action Disconnected;

    public event Action<Node> NodeConnected;
    public event Action<Node> NodeDisconnected;

    public event Action<Node> NodeTx;
    public event Action<Node> NodeRx;

    private readonly ILogger<SvxLinkService> logger;
    private readonly IRepositories repositories;
    private readonly string applicationPath = Directory.GetCurrentDirectory();
    private Timer timer;
    private int channelId;
    private DateTime lastTx;
    

    public SvxLinkService(ILogger<SvxLinkService> logger, IRepositories repositories)
    {
      this.logger = logger;
      this.repositories = repositories;

      NodeTx += n => lastTx = DateTime.Now;
      Connected += () => lastTx = DateTime.Now;
    }

    private void CheckTemporized(object s, ElapsedEventArgs e)
    {
      var diff = (DateTime.Now - lastTx).TotalSeconds;

      logger.LogInformation($"Durée depuis le dernier passage en émission {diff} secondes.");

      if ( diff > 180)
      {
        logger.LogInformation("Delai d'inactivité dépassé. Retour au salon par défaut.");
        Channel = repositories.Channels.GetDefault().Id;
      }
        
    }

    public int Channel
    {
      get => channelId;
      set
      {
        channelId = value;

        switch (value)
        {
          case 0:
            StopSvxlink();
            break;

          case 1000:
            Parrot();
            break;

          default:
            ChangeChannel();
            break;
        }
      }
        
    }

    private void Parrot()
    {
      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      // Remplace le contenu de svxlink.conf avec le informations du channel
      var global = new Dictionary<string, string>
      {
        { "LOGICS", "SimplexLogic" }
      };
      var simplexlogic = new Dictionary<string, string> {
        { "MODULES", "ModuleParrot"}
      };
      var parameters = new Dictionary<string, Dictionary<string, string>>
      {
        {"GLOBAL", global },
        {"SimplexLogic", simplexlogic }
      };
      ReplaceConfig(parameters);
      logger.LogInformation("Remplacement du contenu svxlink.current");

      // suppression du fichier d'annonce
      File.Delete("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav");

      // Lance svxlink
      RunsvxLink();

      System.Threading.Thread.Sleep(1000);

      logger.LogInformation("Séléction du salon perroquet.");
      ExecuteCommand("echo '1#'> /tmp/dtmf_uhf");
    }

    public List<Node> Nodes { get; set; } = new List<Node>();

    private void RunsvxLink()
    {
      var cmd = $"svxlink --pidfile=/var/run/svxlink.pid --runasuser=root --config={applicationPath}/SvxlinkConfig/svxlink.current";

      var escapedArgs = cmd.Replace("\"", "\\\"");

      var shell = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          StandardOutputEncoding = Encoding.UTF8,
          RedirectStandardError = true,
          StandardErrorEncoding = Encoding.UTF8,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      shell.EnableRaisingEvents = true;
      shell.ErrorDataReceived += new DataReceivedEventHandler(ShellErrorDataReceived);
      shell.OutputDataReceived += new DataReceivedEventHandler(ShellOutputDataReceived);

      shell.Start();
      shell.BeginErrorReadLine();
      shell.BeginOutputReadLine();

      var channel = repositories.Channels.Find(channelId);
      if (channel != null)
        SetTimer(channel);
    }

    private void SetTimer(Channel channel)
    {
      timer = new Timer(1000);
      timer.Start();

      if (channel.IsTemporized)
        timer.Elapsed += CheckTemporized;
    }

    /// <summary>Recherche le processus svxlink et le stop.<br />Le timer est stoppé en même temps.</summary>
    private void StopSvxlink()
    {
      logger.LogInformation("Kill de svxlink.");

      timer?.Stop();

      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");
    }

    private void ShellOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    private void ShellErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    private void ChangeChannel()
    {
      logger.LogInformation("Restart salon.");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      // Récupère le channel
      var channel = repositories.Channels.Get(channelId);
      logger.LogInformation($"Recupération du salon {channel.Name}");

      var radioProfile = repositories.RadioProfiles.GetCurrent();

      // Remplace le contenu de svxlink.conf avec le informations du channel
      var global = new Dictionary<string, string>
      {
        { "LOGICS", "SimplexLogic,ReflectorLogic" }
      };
      var simplexlogic = new Dictionary<string, string> {
        { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},
        { "CALLSIGN", channel.ReportCallSign},
        { "REPORT_CTCSS", radioProfile.RxTone}
      };
      var ReflectorLogic = new Dictionary<string, string>
      {
        {"CALLSIGN", channel.CallSign },
        {"HOST", channel.Host },
        {"AUTH_KEY",channel.AuthKey },
        {"PORT" ,channel.Port.ToString()}

      };
      var parameters = new Dictionary<string, Dictionary<string, string>> 
      {
        {"GLOBAL", global },
        {"SimplexLogic", simplexlogic },
        {"ReflectorLogic" , ReflectorLogic}
      };
      ReplaceConfig(parameters);
      logger.LogInformation("Remplacement du contenu svxlink.current");

      // cahngement du son de l'annonce
      if (!Directory.Exists("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager"))
        Directory.CreateDirectory("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager");

      File.Copy($"{applicationPath}/Sounds/{channel.SoundName}", "/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav", true);
      logger.LogInformation("Remplacement du fichier wav d'annonce.");

      // Lance svxlink
      RunsvxLink();
      logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    private void ParseLog(string s)
    {
      if (string.IsNullOrEmpty(s))
        return;

      if (s.Contains("Connected nodes"))
      {
        Nodes.Clear();
        s.Split(':')[2].Split(',').ToList().ForEach(n => Nodes.Add(new Node { Name = n }));
        Connected?.Invoke();
      }

      if (s.Contains("SIGTERM"))
      {
        Nodes.Clear();
        Disconnected?.Invoke();
      }

      if (s.Contains("Node left"))
      {
        var node = new Node { Name = s.Split(":")[2] };
        Nodes.Remove(node);
        NodeDisconnected?.Invoke(node);
      }
        
      if (s.Contains("Node joined"))
      {
        var node = new Node { Name = s.Split(":")[2] };
        Nodes.Add(node);
        NodeConnected?.Invoke(node);
      }

      if (s.Contains("Talker start"))
      {
        var node = Nodes.Single(nx => nx.Equals(new Node { Name = s.Split(":")[2] }));
        node.ClassName = "node node-tx";
        NodeTx?.Invoke(node);
      }
        
      if (s.Contains("Talker stop"))
      {
        var node = Nodes.Single(nx => nx.Equals(new Node { Name = s.Split(":")[2] }));
        node.ClassName = "node";
        NodeRx?.Invoke(node);
      }
        
    }

    private void ReplaceConfig(Dictionary<string, Dictionary<string,string>> parameters)
    {
      var parser = new FileIniDataParser();
      parser.Parser.Configuration.NewLineStr = "\r\n";
      parser.Parser.Configuration.AssigmentSpacer = string.Empty;

      var utf8WithoutBom = new UTF8Encoding(false);

      var data = parser.ReadFile($"{applicationPath}/SvxlinkConfig/svxlink.conf", utf8WithoutBom);

      foreach (var section in parameters)
        foreach (var parameter in section.Value)
          data[section.Key][parameter.Key] = parameter.Value;
        
      parser.WriteFile($"{applicationPath}/SvxlinkConfig/svxlink.current", data, utf8WithoutBom);

    }

    private string ExecuteCommand(string cmd)
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
