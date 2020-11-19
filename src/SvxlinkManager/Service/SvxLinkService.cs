using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Repositories;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private readonly Timer timer;
    private int channelId;
    private DateTime lastTx;
    

    public SvxLinkService(ILogger<SvxLinkService> logger, IRepositories repositories)
    {
      this.logger = logger;
      this.repositories = repositories;

      timer = new Timer(1000);
      timer.Elapsed += CheckDtmf;
      
      NodeTx += n => lastTx = DateTime.Now;
      Connected += () => lastTx = DateTime.Now;
    }

    private void CheckDtmf(object s, ElapsedEventArgs e)
    {
      var dtmf = int.Parse(File.ReadAllText($"{applicationPath}/SvxlinkConfig/dtmf.conf"));

      var channel = repositories.Channels.Get(channelId);

      if (channel.Dtmf != dtmf)
      {
        logger.LogInformation($"Nouveau dtmf détécté: {dtmf}");
        channel = repositories.Channels.FindBy(c => c.Dtmf == dtmf);
        if(channel == null)
        {
          logger.LogInformation($"Le dtmf {dtmf} ne correspond à aucun channel.");
          return;
        }

        Channel = channel.Id;
      }
        
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
        RunRestart();
      }
        
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

      var channel = repositories.Channels.Get(channelId);
      timer.Start();
      if(channel.IsTemporized)
        timer.Elapsed += CheckTemporized;
    }

    /// <summary>Recherche le processus svxlink et le stop.<br />Le timer est stoppé en même temps.</summary>
    private void StopSvxlink()
    {
      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");

      timer.Elapsed -= CheckTemporized;
      timer.Stop();
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

    private void RunRestart()
    {
      logger.LogInformation("Restart salon.");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      // Si le choix est Déconnecter, fin de la méthode
      if (channelId == 0)
        return;

      // Récupère le channel
      var channel = repositories.Channels.Get(channelId);
      logger.LogInformation($"Recupération du salon {channel.Name}");

      var radioProfile = repositories.RadioProfiles.GetCurrent();

      // Remplace le contenu de svxlink.conf avec le informations du channel
      ReplaceConfig(channel, radioProfile);
      logger.LogInformation("Remplacement du contenu svxlink.current");

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

    private void ReplaceConfig(Channel channel, RadioProfile radioProfile)
    {
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/dtmf.conf", channel.Dtmf.ToString());

      File.Copy($"{applicationPath}/SvxlinkConfig/svxlink.conf", $"{applicationPath}/SvxlinkConfig/svxlink.current", true);

      string text = File.ReadAllText($"{applicationPath}/SvxlinkConfig/svxlink.current");

      text = text.Replace("HOST=HOST", $"HOST={channel.Host}");
      text = text.Replace("AUTH_KEY=AUTH_KEY", $"AUTH_KEY={channel.AuthKey}");
      text = text.Replace("PORT=PORT", $"PORT={channel.Port}");
      text = text.Replace("CALLSIGN=CALLSIGN", $"CALLSIGN={channel.CallSign}");
      text = text.Replace("CALLSIGN=REPORTCALLSIGN", $"CALLSIGN={channel.ReportCallSign}");
      text = text.Replace("REPORT_CTCSS=REPORT_CTCSS", $"REPORT_CTCSS={radioProfile.RxTone}");

      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.current", text);
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
