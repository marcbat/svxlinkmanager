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
    #region Fields

    private readonly string applicationPath = Directory.GetCurrentDirectory();

    private readonly ILogger<SvxLinkService> logger;

    private readonly IRepositories repositories;

    private int channelId;

    /// <summary>Datetime of the last TX on the current channel.</summary>
    private DateTime lastTx;

    private Timer timer;

    private FileSystemWatcher watcher;

    #endregion Fields

    #region Constructors

    public SvxLinkService(ILogger<SvxLinkService> logger, IRepositories repositories)
    {
      this.logger = logger;
      this.repositories = repositories;

      NodeTx += n => lastTx = DateTime.Now;
      Connected += () => lastTx = DateTime.Now;
    }

    #endregion Constructors

    #region Events

    /// <summary>Occurs when svxlink is connected</summary>
    public event Action Connected;
    /// <summary>Occurs when svxlink is disconnected</summary>
    public event Action Disconnected;

    /// <summary>Occurs when a new node join the channel</summary>
    public event Action<Node> NodeConnected;
    /// <summary>Occurs when a node quit the channel</summary>
    public event Action<Node> NodeDisconnected;

    /// <summary>Occurs when a node stop a transmission.</summary>
    public event Action<Node> NodeRx;

    /// <summary>Occurs when a node start a transmission.</summary>
    public event Action<Node> NodeTx;

    #endregion Events

    #region Properties

    /// <summary>Gets or sets the current channel Id</summary>
    /// <value>Current channel Id</value>
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

    /// <summary>List of connected nodes</summary>
    /// <value>connected nodes</value>
    public List<Node> Nodes { get; set; } = new List<Node>();

    /// <summary>Current channel status</summary>
    /// <value>Current status</value>
    public string Status { get; set; } = "Déconnecté";

    #endregion Properties

    #region Methods

    /// <summary>Execute a cli command</summary>
    /// <param name="cmd">The command.</param>
    /// <returns>Console output</returns>
    private static string ExecuteCommand(string cmd)
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

    /// <summary>
    ///   <para>
    /// Changes the Current channel.</para>
    ///   <para>Group Stop, replace config and start svxlink.</para>
    /// </summary>
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
      StartSvxLink();
      logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    /// <summary>Checks if the channel temporisation is exceeded</summary>
    /// <param name="s">Timer</param>
    /// <param name="e">The <see cref="ElapsedEventArgs" /> instance containing the event data.</param>
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
    /// <summary>Activate the Parrot module</summary>
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
      StartSvxLink();

      System.Threading.Thread.Sleep(1000);

      logger.LogInformation("Séléction du salon perroquet.");
      ExecuteCommand("echo '1#'> /tmp/dtmf_uhf");
    }

    /// <summary>Parse the Svxlink logs</summary>
    /// <param name="s">one Log line</param>
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

    /// <summary>Replace parameters int svxlink.current ini file</summary>
    /// <param name="parameters">Dictionnary of parameters</param>
    /// <example>
    ///   <para> var global = new Dictionary&lt;string, string&gt;<br />      {<br />        { "LOGICS", "SimplexLogic,ReflectorLogic" }<br />      };<br />      var simplexlogic = new Dictionary&lt;string, string&gt; {<br />        { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},<br />        { "CALLSIGN", channel.ReportCallSign},<br />        { "REPORT_CTCSS", radioProfile.RxTone}<br />      };<br />      var ReflectorLogic = new Dictionary&lt;string, string&gt;<br />      {<br />        {"CALLSIGN", channel.CallSign },<br />        {"HOST", channel.Host },<br />        {"AUTH_KEY",channel.AuthKey },<br />        {"PORT" ,channel.Port.ToString()}</para>
    ///   <para>      };<br />      var parameters = new Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt; <br />      {<br />        {"GLOBAL", global },<br />        {"SimplexLogic", simplexlogic },<br />        {"ReflectorLogic" , ReflectorLogic}<br />      };<br /></para>
    ///   <code></code>
    /// </example>
    private void ReplaceConfig(Dictionary<string, Dictionary<string, string>> parameters)
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

    /// <summary>Sets the file watcher. This watcher monitor dtmf.conf file.</summary>
    private void SetDtmfWatcher()
    {
      var dtmfFilePath = $"{applicationPath}/SvxlinkConfig/dtmf.conf";

      watcher = new FileSystemWatcher
      {
        Path = Path.GetDirectoryName(dtmfFilePath),
        Filter = Path.GetFileName(dtmfFilePath)
      };

      watcher.Changed += (s, e) =>
      {
        logger.LogInformation("Changement de dtmf détécté.");
        var dtmf = File.ReadAllText(dtmfFilePath);
        logger.LogInformation($"Nouveau dtmf {dtmf}");

        var channel = repositories.Channels.FindBy(c => c.Dtmf == Int32.Parse(dtmf));
        if (channel == null)
        {
          logger.LogInformation($"Le dtmf {dtmf} ne correspond à aucun channel.");
          return;
        }

        Channel = channel.Id;
      };

      watcher.EnableRaisingEvents = true;
    }

    /// <summary>Set the temporized timer for current channel</summary>
    private void SetTimer()
    {
      timer = new Timer(1000);
      timer.Start();

      timer.Elapsed += CheckTemporized;
    }

    /// <summary>Log Svxlink error output data</summary>
    /// <param name="sender">shell</param>
    /// <param name="e">The <see cref="DataReceivedEventArgs" /> instance containing the event data.</param>
    private void ShellErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    /// <summary>Log Svxlink output data</summary>
    /// <param name="sender">shell</param>
    /// <param name="e">The <see cref="DataReceivedEventArgs" /> instance containing the event data.</param>
    private void ShellOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    /// <summary>Starts the SVXlink application with svxlink.current configuration</summary>
    private void StartSvxLink()
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
      if (channel != null && channel.IsTemporized)
        SetTimer();

      SetDtmfWatcher();

      Status = "Connecté";
    }
    /// <summary>Stops the svxlink application. <br />Kill the temporised timer and the dtmf file watcher</summary>
    private void StopSvxlink()
    {
      logger.LogInformation("Kill de svxlink.");

      timer?.Stop();
      watcher?.Dispose();

      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");

      Status = "Déconnecté";
    }

    #endregion Methods
  }
}
