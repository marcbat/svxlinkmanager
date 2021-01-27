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
    public readonly string applicationPath = Directory.GetCurrentDirectory();

    private readonly ILogger<SvxLinkService> logger;

    private readonly IRepositories repositories;
    private readonly ScanService scanService;
    private int channelId;

    /// <summary>
    /// Datetime of the last TX on the current channel.
    /// </summary>
    private DateTime lastTx;

    private Timer tempoTimer;
    private Timer scanTimer;

    private FileSystemWatcher watcher;
    private Process shell;

    public SvxLinkService(ILogger<SvxLinkService> logger, IRepositories repositories, ScanService scanService)
    {
      this.logger = logger;
      this.repositories = repositories;
      this.scanService = scanService;
      NodeTx += n => lastTx = DateTime.Now;
      Connected += (c) => lastTx = DateTime.Now;
    }

    /// <summary>
    /// Occurs when svxlink is connected
    /// </summary>
    public event Action<Channel> Connected;

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

    /// <summary>
    /// Occurs when countdown change
    /// </summary>
    public event Action<double> TempChanged;

    /// <summary>
    /// Gets or sets the current channel Id
    /// </summary>
    /// <value>Current channel Id</value>
    public int ChannelId
    {
      get => channelId;
      set
      {
        channelId = value;

        try
        {
          switch (value)
          {
            case 0:
              StopSvxlink();
              break;

            case 1000:
              Parrot();
              break;

            default:
              ActivateChannel(value);
              break;
          }
        }
        catch (Exception e)
        {
          Error?.Invoke("Impossible de changer de salon.", e.Message);
        }
      }
    }

    /// <summary>
    /// List of connected nodes
    /// </summary>
    /// <value>connected nodes</value>
    public List<Node> Nodes { get; set; } = new List<Node>();

    /// <summary>
    /// Current channel status
    /// </summary>
    /// <value>Current status</value>
    public string Status { get; set; } = "Déconnecté";

    /// <summary>Connecte le salon par défaut.</summary>
    public virtual void StartDefaultChannel() =>
      ChannelId = repositories.Channels.GetDefault().Id;

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

    /// <summary>Test channel type and activate</summary>
    /// <param name="channelid">Channel Id</param>
    /// <exception cref="Exception">Impossible de trouver le type de channel.</exception>
    public virtual void ActivateChannel(int channelid)
    {
      var channel = repositories.Channels.Get(channelid);

      switch (channel)
      {
        case SvxlinkChannel c:
          ActivateSvxlinkChannel(c);
          break;

        case EcholinkChannel c:
          ActivateEcholink(c);
          break;

        default:
          throw new Exception("Impossible de trouver le type de channel.");
      }
    }

    /// <summary>
    /// <para>Changes the Current channel.</para>
    /// <para>Group Stop, replace config and start svxlink.</para>
    /// </summary>
    public virtual void ActivateSvxlinkChannel(SvxlinkChannel channel)
    {
      logger.LogInformation("Restart salon.");

      if (channel?.CallSign == "(CH) SVX4LINK H")
      {
        ChannelId = 0;
        Error?.Invoke("Attention", "Vous ne pouvez pas vous connecter avec le call par défaut. <br/> Merci de le changer dans la configuration des salons.");
        return;
      }

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      var radioProfile = repositories.RadioProfiles.GetCurrent();

      // Remplacement de la config
      CreateNewCurrentConfig();

      var global = new Dictionary<string, string>
      {
        { "LOGICS", "SimplexLogic,ReflectorLogic" }
      };
      var simplexlogic = new Dictionary<string, string> {
        { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},
        { "CALLSIGN", channel.ReportCallSign},
        { "REPORT_CTCSS", radioProfile.RxTone}
      };
      var rx = new Dictionary<string, string>
      {
        {"SQL_DET", radioProfile.SquelchDetection },
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
        {"Rx1", rx},
        {"ReflectorLogic" , ReflectorLogic}
      };

      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.current", parameters);
      logger.LogInformation("Remplacement du contenu svxlink.current");

      ReplaceSoundFile(channel);

      // Lance svxlink
      StartSvxLink(channel);
      logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    /// <summary>Replaces the sound file for the channel</summary>
    /// <param name="channel">The channel.</param>
    protected virtual void ReplaceSoundFile(Channel channel = null)
    {
      logger.LogInformation("Remplacement du fichier wav d'annonce.");

      if (channel == null)
      {
        File.Delete("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav");
        return;
      }

      if (!Directory.Exists("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager"))
        Directory.CreateDirectory("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager");

      if (!string.IsNullOrEmpty(channel.SoundName))
        File.Copy($"{applicationPath}/Sounds/{channel.SoundName}", "/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav", true);
    }

    /// <summary>Activates an Echolink channel</summary>
    /// <param name="channel">The Echolink channel</param>
    public virtual void ActivateEcholink(EcholinkChannel channel)
    {
      logger.LogInformation("Restart link echolink.");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      var radioProfile = repositories.RadioProfiles.GetCurrent();

      // Remplacement de la config
      CreateNewCurrentConfig();

      var global = new Dictionary<string, string>
      {
        { "LOGICS", "SimplexLogic" }
      };
      var simplexlogic = new Dictionary<string, string> {
        { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor,ModuleEchoLink,ModuleParrot"},
        { "CALLSIGN", channel.CallSign},
        { "REPORT_CTCSS", radioProfile.RxTone}
      };
      var parameters = new Dictionary<string, Dictionary<string, string>>
      {
        {"GLOBAL", global },
        {"SimplexLogic", simplexlogic }
      };

      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.current", parameters);
      logger.LogInformation("Remplacement du contenu svxlink.current");

      var moduleEcholink = new Dictionary<string, string>
      {
        {"SERVERS", channel.Host },
        {"CALLSIGN", channel.CallSign },
        {"PASSWORD", channel.Password },
        {"SYSOPNAME", channel.SysopName },
        {"LOCATION", channel.Location },
        {"MAX_QSOS", channel.MaxQso.ToString() },
        {"MAX_CONNECTIONS", (channel.MaxQso + 1).ToString() },
        {"DESCRIPTION", channel.Description },
      };
      parameters = new Dictionary<string, Dictionary<string, string>>
      {
        {"ModuleEchoLink", moduleEcholink },
      };
      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleEchoLink.conf", parameters);
      logger.LogInformation("Remplacement du contenu de ModuleEcholink.conf");

      // Remplacement du son de l'annonce
      ReplaceSoundFile(channel);

      // Ajout du link en tant que Node
      Nodes.Add(new Node { Name = channel.CallSign });

      // Lance svxlink
      StartSvxLink(channel);
      logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    /// <summary>
    /// Checks if the channel temporisation is exceeded
    /// </summary>
    /// <param name="s">Timer</param>
    /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
    protected virtual void CheckTemporized(object s, ElapsedEventArgs e)
    {
      var diff = (DateTime.Now - lastTx).TotalSeconds;
      var channel = repositories.Channels.Get(channelId);

      logger.LogInformation($"Durée depuis le dernier passage en émission {diff} secondes.");

      TempChanged?.Invoke(channel.TimerDelay - diff);

      if (diff < channel.TimerDelay)
        return;

      logger.LogInformation("Delai d'inactivité dépassé. Retour au salon par défaut.");
      ChannelId = repositories.Channels.GetDefault().Id;
    }

    protected virtual void CheckScan(object sender, ElapsedEventArgs e)
    {
      var diff = (DateTime.Now - lastTx).TotalSeconds;
      var scanProfile = repositories.ScanProfiles.Get(1);

      logger.LogInformation($"Durée depuis le dernier passage en émission {diff} secondes.");

      if (diff < scanProfile.ScanDelay)
        return;

      var activeChannel = scanService.GetActiveChannel(scanProfile);
      if (activeChannel != null)
        ChannelId = activeChannel.Id;
    }

    /// <summary>
    /// Activate the Parrot module
    /// </summary>
    public virtual void Parrot()
    {
      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      CreateNewCurrentConfig();

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

      // Remplace le contenu de svxlink.conf avec le informations du channel
      logger.LogInformation("Remplacement du contenu svxlink.current");
      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.current", parameters);

      // suppression du fichier d'annonce
      ReplaceSoundFile();

      // Lance svxlink
      StartSvxLink();

      System.Threading.Thread.Sleep(1000);

      logger.LogInformation("Séléction du salon perroquet.");
      ExecuteCommand("echo '1#'> /tmp/dtmf_uhf");
    }

    /// <summary>
    /// Copy svxlink.conf vers svxlink.current
    /// </summary>
    protected virtual void CreateNewCurrentConfig() =>
      File.Copy($"{applicationPath}/SvxlinkConfig/svxlink.conf", $"{applicationPath}/SvxlinkConfig/svxlink.current", true);

    /// <summary>
    /// Parse the Svxlink logs
    /// </summary>
    /// <param name="s">one Log line</param>
    public virtual void ParseLog(Channel channel, string s)
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

      if (s.Contains("SIGTERM"))
      {
        Nodes.Clear();
        Disconnected?.Invoke();
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
        Error?.Invoke("Echec de la connexion.", $"Impossible de se connecter au salon {channel.Name}. <br/> Accès refusé.");
        return;
      }

      if (s.Contains("Host not found"))
      {
        Error?.Invoke("Echec de la connexion.", $"Impossible de se connecter au salon {channel.Name}. <br/> Server {channel.Host} introuvable.");
        return;
      }
    }

    /// <summary>
    /// Replace parameters int svxlink.current ini file
    /// </summary>
    /// <param name="parameters">Dictionnary of parameters</param>
    /// <example>
    /// <para>
    /// var global = new Dictionary&lt;string, string&gt; <br/> { <br/> { "LOGICS",
    /// "SimplexLogic,ReflectorLogic" } <br/> }; <br/> var simplexlogic = new Dictionary&lt;string,
    /// string&gt; { <br/> { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},
    /// <br/> { "CALLSIGN", channel.ReportCallSign}, <br/> { "REPORT_CTCSS", radioProfile.RxTone}
    /// <br/> }; <br/> var ReflectorLogic = new Dictionary&lt;string, string&gt; <br/> { <br/>
    /// {"CALLSIGN", channel.CallSign }, <br/> {"HOST", channel.Host }, <br/>
    /// {"AUTH_KEY",channel.AuthKey }, <br/> {"PORT" ,channel.Port.ToString()}
    /// </para>
    /// <para>
    /// }; <br/> var parameters = new Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt;
    /// <br/> { <br/> {"GLOBAL", global }, <br/> {"SimplexLogic", simplexlogic }, <br/>
    /// {"ReflectorLogic" , ReflectorLogic} <br/> }; <br/>
    /// </para>
    /// <code></code>
    /// </example>
    protected virtual void ReplaceConfig(string filePath, Dictionary<string, Dictionary<string, string>> parameters)
    {
      var parser = new FileIniDataParser();
      parser.Parser.Configuration.NewLineStr = "\r\n";
      parser.Parser.Configuration.AssigmentSpacer = string.Empty;

      var utf8WithoutBom = new UTF8Encoding(false);

      var data = parser.ReadFile(filePath, utf8WithoutBom);

      foreach (var section in parameters)
        foreach (var parameter in section.Value)
          data[section.Key][parameter.Key] = parameter.Value;

      parser.WriteFile(filePath, data, utf8WithoutBom);
    }

    /// <summary>
    /// Sets the file watcher. This watcher monitor dtmf.conf file.
    /// </summary>
    protected virtual void SetDtmfWatcher()
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

        var dtmfChannel = repositories.Channels.FindBy(c => c.Dtmf == Int32.Parse(dtmf));
        if (dtmfChannel == null)
        {
          logger.LogInformation($"Le dtmf {dtmf} ne correspond à aucun channel.");
          return;
        }

        ChannelId = dtmfChannel.Id;
      };

      watcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Set the temporized timer for current channel
    /// </summary>
    protected virtual void SetTimer()
    {
      tempoTimer = new Timer(1000);
      tempoTimer.Start();

      tempoTimer.Elapsed += CheckTemporized;
    }

    protected virtual void SetScanTimer()
    {
      scanTimer = new Timer(5000);
      scanTimer.Start();

      scanTimer.Elapsed += CheckScan;
    }

    /// <summary>
    /// Starts the SVXlink application with svxlink.current configuration
    /// </summary>
    public virtual void StartSvxLink(Channel channel = null)
    {
      var cmd = $"svxlink --pidfile=/var/run/svxlink.pid --runasuser=root --config={applicationPath}/SvxlinkConfig/svxlink.current";

      var escapedArgs = cmd.Replace("\"", "\\\"");

      shell = new Process()
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

      var scanProfil = repositories.ScanProfiles.Get(1);

      if (channel != null && channel.IsTemporized)
        SetTimer();

      if (channel != null && scanProfil.Enable)
        SetScanTimer();

      SetDtmfWatcher();

      Status = "Connecté";
    }

    /// <summary>
    /// Stops the svxlink application. <br/> Kill the temporised timer and the dtmf file watcher
    /// </summary>
    public virtual void StopSvxlink()
    {
      logger.LogInformation("Kill de svxlink.");

      tempoTimer?.Stop();
      watcher?.Dispose();

      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");

      shell?.Dispose();
      Status = "Déconnecté";
    }
  }
}