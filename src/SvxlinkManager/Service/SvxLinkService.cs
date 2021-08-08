using IniParser;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Common.Models;
using SvxlinkManager.Common.Service;
using SvxlinkManager.Models;
using SvxlinkManager.Repositories;
using SvxlinkManager.Telemetry;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace SvxlinkManager.Service
{
  public class SvxLinkService : SvxlinkServiceBase
  {
    public readonly string applicationPath = Directory.GetCurrentDirectory();

    private readonly ILogger<SvxLinkService> logger;

    private readonly IRepositories repositories;
    private readonly ScanService scanService;
    private readonly TelemetryClient telemetry;
    private int channelId;

    /// <summary>
    /// Datetime of the last TX on the current channel.
    /// </summary>
    private DateTime lastTx;

    private Timer tempoTimer;
    private Timer scanTimer;

    private FileSystemWatcher watcher;

    internal void StopReflector()
    {
      throw new NotImplementedException();
    }

    private Process shell;

    internal void RunReflector()
    {
      throw new NotImplementedException();
    }

    public SvxLinkService(ILogger<SvxLinkService> logger, IRepositories repositories, ScanService scanService, TelemetryClient telemetry) : base(logger, telemetry)
    {
      this.logger = logger;
      this.repositories = repositories;
      this.scanService = scanService;
      this.telemetry = telemetry;

      lastTx = DateTime.Now;

      tempoTimer = new Timer(1000)
      {
        Enabled = false
      };
      tempoTimer.Start();

      tempoTimer.Elapsed += CheckTemporized;

      scanTimer = new Timer(5000)
      {
        Enabled = false
      };
      scanTimer.Start();

      scanTimer.Elapsed += CheckScan;
    }

    public event Action StartTempo;

    public event Action StopTempo;

    /// <summary>
    /// Occurs when countdown change
    /// </summary>
    public event Action<string> TempChanged;

    /// <summary>
    /// Occurs when temporisation limit is out.
    /// </summary>
    public event Action TempoQsy;

    /// <summary>
    /// Occurs when scanning.
    /// </summary>
    public event Action Scanning;

    public event Action StopScanning;

    /// <summary>
    /// Occurs when scanning make a QSY.
    /// </summary>
    public event Action ScanningQsy;

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
          telemetry.TrackException(e, new Dictionary<string, string> { { "ChannelId", value.ToString() } });
          OnError("Impossible de changer de salon.", e.Message);
        }
      }
    }

    /// <summary>
    /// Current channel status
    /// </summary>
    /// <value>Current status</value>
    public string Status { get; set; } = "Déconnecté";

    /// <summary>Connecte le salon par défaut.</summary>
    public virtual void StartDefaultChannel()
    {
      var defaultId = repositories.Channels.GetDefault()?.Id;
      if (defaultId != null)
        ChannelId = (int)defaultId;
    }

    /// <summary>Test channel type and activate</summary>
    /// <param name="channelid">Channel Id</param>
    /// <exception cref="Exception">Impossible de trouver le type de channel.</exception>
    public virtual void ActivateChannel(int channelid)
    {
      var channel = repositories.Channels.GetWithSound(channelid);

      switch (channel)
      {
        case SvxlinkChannel c:
          ActivateSvxlinkChannel(c);
          break;

        case EcholinkChannel c:
          ActivateEcholink(c);
          break;

        case AdvanceSvxlinkChannel c:
          ActivateAdvanceSvxlinkChannel(c);
          break;

        default:
          throw new Exception($"Impossible de trouver le type de channel. {channel.GetType()} ");
      }
    }

    /// <summary>
    /// <para>Changes the Current channel.</para>
    /// <para>Group Stop, replace config and start svxlink.</para>
    /// </summary>
    public virtual void ActivateSvxlinkChannel(SvxlinkChannel channel)
    {
      var url = new UriBuilder("http", channel.Host, channel.Port).Uri;

      var dependencyTracker = new DependencyTelemetry
      {
        Id = Guid.NewGuid().ToString(),
        Name = "ActivateSvxlinkChannel",
        Data = url.AbsolutePath,
        Target = url.Authority,
        Type = "http"
      };

      using (var operation = telemetry.StartOperation(dependencyTracker))
      {
        logger.LogInformation("Restart salon.");

        if (channel?.CallSign == "(CH) SVX4LINK H")
        {
          telemetry.TrackTrace("Vous ne pouvez pas vous connecter avec le call par défaut. <br/> Merci de le changer dans la configuration des salons.", SeverityLevel.Error, channel.TrackProperties);

          ChannelId = 0;
          OnError("Attention", "Vous ne pouvez pas vous connecter avec le call par défaut. <br/> Merci de le changer dans la configuration des salons.");
          return;
        }

        // Stop svxlink
        StopSvxlink();
        logger.LogInformation("Salon déconnecté");

        var radioProfile = repositories.RadioProfiles.GetCurrent();

        Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");
        File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", repositories.Parameters.GetStringValue("default.svxlink.conf"));

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
        {"CTCSS_FQ", radioProfile.RxTone }
      };
        var tx = new Dictionary<string, string>
      {
        {"CTCSS_FQ", radioProfile.TxTone }
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
        { "Tx1", tx },
        {"ReflectorLogic" , ReflectorLogic}
      };

        ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.conf", parameters);
        logger.LogInformation("Remplacement du contenu svxlink.conf");

        ReplaceSoundFile(channel);

        // Lance svxlink
        StartSvxLink(channel);
        logger.LogInformation($"Le channel {channel.Name} est connecté.");
      }
    }

    private void ActivateAdvanceSvxlinkChannel(AdvanceSvxlinkChannel channel)
    {
      logger.LogInformation("restart advance salon.");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");

      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", channel.SvxlinkConf);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleDtmfRepeater.conf", channel.ModuleDtmfRepeater);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleEchoLink.conf", channel.ModuleEchoLink);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleFrn.conf", channel.ModuleFrn);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleHelp.conf", channel.ModuleHelp);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleMetarInfo.conf", channel.ModuleMetarInfo);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleParrot.conf", channel.ModuleParrot);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModulePropagationMonitor.conf", channel.ModulePropagationMonitor);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleSelCallEnc.conf", channel.ModuleSelCallEnc);
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleTclVoiceMail.conf", channel.ModuleTclVoiceMail);

      ReplaceSoundFile(channel);

      // Lance svxlink
      StartSvxLink(channel);
      logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    /// <summary>Replaces the sound file for the channel</summary>
    /// <param name="channel">The channel.</param>
    protected virtual void ReplaceSoundFile(ManagedChannel channel = null)
    {
      logger.LogInformation("Remplacement du fichier wav d'annonce.");

      if (channel == null)
      {
        File.Delete("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav");
        return;
      }

      if (!Directory.Exists("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager"))
        Directory.CreateDirectory("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager");

      if (!string.IsNullOrEmpty(channel.Sound.SoundName))
      {
        File.Delete("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav");
        File.WriteAllBytes("/usr/share/svxlink/sounds/fr_FR/svxlinkmanager/Name.wav", channel.Sound.SoundFile);
      }
    }

    /// <summary>Activates an Echolink channel</summary>
    /// <param name="channel">The Echolink channel</param>
    public virtual void ActivateEcholink(EcholinkChannel channel)
    {
      telemetry.TrackEvent("Start echolink", new Dictionary<string, string> { { "linkName", channel.Name } });

      logger.LogInformation("Restart link echolink.");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      var radioProfile = repositories.RadioProfiles.GetCurrent();

      Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", repositories.Parameters.GetStringValue("default.svxlink.conf"));
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.d/ModuleEchoLink.conf", repositories.Parameters.GetStringValue("default.echolink.conf"));

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

      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.conf", parameters);
      logger.LogInformation("Remplacement du contenu svxlink.conf");

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
      if (channelId == 0)
        return;

      var diff = (DateTime.Now - lastTx).TotalSeconds;
      var channel = repositories.Channels.Get(channelId);

      logger.LogDebug($"Durée depuis le dernier passage en émission {diff} secondes.");

      var countdown = TimeSpan.FromSeconds(channel.TimerDelay - diff);
      TempChanged?.Invoke(countdown.ToString(@"mm\:ss"));

      if (diff < channel.TimerDelay)
        return;

      TempoQsy?.Invoke();

      logger.LogInformation("Delai d'inactivité dépassé. Retour au salon par défaut.");

      var defaultChannel = repositories.Channels.GetDefault();

      telemetry.TrackEvent("Temporized QSY", defaultChannel.TrackProperties);

      ChannelId = defaultChannel.Id;
    }

    protected virtual void CheckScan(object sender, ElapsedEventArgs e)
    {
      var diff = (DateTime.Now - lastTx).TotalSeconds;
      var scanProfile = repositories.ScanProfiles.Get(1);

      if (diff < scanProfile.ScanDelay)
      {
        logger.LogInformation($"Le scan delay de {scanProfile.ScanDelay} n'a pas encore été atteint.");
        return;
      }

      Scanning?.Invoke();

      var activeChannel = scanService.GetActiveChannel(scanProfile);
      if (activeChannel != null && activeChannel.Id != channelId)
      {
        ScanningQsy?.Invoke();

        telemetry.TrackEvent("Scan QSY", activeChannel.TrackProperties);
        ChannelId = activeChannel.Id;
      }
    }

    /// <summary>
    /// Activate the Parrot module
    /// </summary>
    public virtual void Parrot()
    {
      telemetry.TrackEvent("Start parrot");

      // Stop svxlink
      StopSvxlink();
      logger.LogInformation("Salon déconnecté");

      Directory.CreateDirectory($"{applicationPath}/SvxlinkConfig/svxlink.d");
      File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxlink.conf", repositories.Parameters.GetStringValue("default.svxlink.conf"));

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
      logger.LogInformation("Remplacement du contenu svxlink.conf");
      ReplaceConfig($"{applicationPath}/SvxlinkConfig/svxlink.conf", parameters);

      // suppression du fichier d'annonce
      ReplaceSoundFile();

      // Lance svxlink
      StartSvxLink(new SvxlinkChannel { Name = "Parrot" });

      System.Threading.Thread.Sleep(1000);

      logger.LogInformation("Séléction du salon perroquet.");
      ExecuteCommand("echo '1#'> /tmp/dtmf_uhf");
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
        telemetry.TrackEvent("DTMF change");

        logger.LogInformation("Changement de dtmf détécté.");
        var dtmf = File.ReadAllText(dtmfFilePath);
        logger.LogInformation($"Nouveau dtmf {dtmf}");

        var dtmfChannel = repositories.Channels.FindBy(c => c.Dtmf == Int32.Parse(dtmf));
        if (dtmfChannel == null)
        {
          logger.LogWarning("Le dtmf {dtmf} ne correspond à aucun channel.", dtmf);
          return;
        }

        ChannelId = dtmfChannel.Id;
      };

      watcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Set the temporized timer for current channel
    /// </summary>
    protected virtual void StartTemporization(Node node = null)
    {
      telemetry.TrackEvent("Start temporization", new Dictionary<string, string> { { "node", node?.Name } });

      logger.LogInformation($"La temporisation a été enclenchée par {node?.Name}.");

      lastTx = DateTime.Now;
      tempoTimer.Enabled = true;

      StartTempo?.Invoke();
    }

    protected virtual void StopTemporization(Node node = null)
    {
      telemetry.TrackEvent("Stop temporization", new Dictionary<string, string> { { "node", node?.Name } });

      logger.LogInformation($"La temporisation a été stoppée par {node?.Name}.");

      lastTx = DateTime.Now;
      tempoTimer.Enabled = false;

      StopTempo?.Invoke();
    }

    protected virtual void StartScan(Node node = null)
    {
      telemetry.TrackEvent("Start scan", new Dictionary<string, string> { { "node", node?.Name } });

      logger.LogDebug("Le scan a été enclenchée par {nodeName}.", node?.Name);

      lastTx = DateTime.Now;
      scanTimer.Enabled = true;
    }

    protected virtual void StopScan(Node node = null)
    {
      telemetry.TrackEvent("Stop scan", new Dictionary<string, string> { { "node", node?.Name } });

      logger.LogInformation($"Le scan a été stoppée par {node?.Name}.");

      lastTx = DateTime.Now;
      scanTimer.Enabled = false;

      StopScanning?.Invoke();
    }

    /// <summary>
    /// Starts the SVXlink application with svxlink.current configuration
    /// </summary>
    public virtual void StartSvxLink(ManagedChannel channel)
    {
      telemetry.TrackEvent("Channel Connection", channel.TrackProperties);

      logger.LogInformation("Connection au channel {ChannelName}.", channel.Name);

      base.StartSvxlink(channel, pidFile: "/var/run/svxlink.pid", runAs: "root", configFile: $"{applicationPath}/SvxlinkConfig/svxlink.conf");

      var scanProfil = repositories.ScanProfiles.Get(1);

      if (channel.IsTemporized)
      {
        StartTemporization();
        NodeTx += StopTemporization;
        NodeRx += StartTemporization;
      }

      if (scanProfil.Enable)
      {
        StartScan();
        NodeTx += StopScan;
        NodeRx += StartScan;
      }

      SetDtmfWatcher();
    }

    /// <summary>
    /// Stops the svxlink application. <br/> Kill the temporised timer and the dtmf file watcher
    /// </summary>
    public override void StopSvxlink()
    {
      logger.LogInformation("Kill de svxlink.");

      StopTemporization();
      NodeTx -= StopTemporization;
      NodeRx -= StartTemporization;

      StopScan();
      NodeTx -= StopScan;
      NodeRx -= StartScan;

      watcher?.Dispose();

      base.StopSvxlink();
    }
  }
}