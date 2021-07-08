using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public class ScanService
  {
    private readonly ILogger<ScanService> logger;
    private readonly TelemetryClient telemetry;

    public ScanService(ILogger<ScanService> logger, TelemetryClient telemetry)
    {
      this.logger = logger;
      this.telemetry = telemetry;
    }

    public virtual ManagedChannel GetActiveChannel(ScanProfile scanProfile)
    {
      foreach (var channel in scanProfile.Channels)
      {
        try
        {
          logger.LogDebug($"Scanning du channel {channel.Name}.");

          using var client = new WebClient();
          string response = client.DownloadString(channel.TrackerUrl);
          var tracker = JsonConvert.DeserializeObject<Tracker>(response);

          if (tracker?.Metadata.First().Tot > 3)
          {
            logger.LogInformation($"Le channel {channel.Name} est activé par {tracker?.Metadata.First().Indicatif}");
            return channel;
          }

          logger.LogInformation($"Le channel {channel.Name} est inactif.");
        }
        catch (Exception e)
        {
          logger.LogError($"Erreur lors du scan du channel {channel.Name}.", e);
          telemetry.TrackException(e, new Dictionary<string, string> { { "ChannelName", channel.Name } });
        }
      }

      return null;
    }
  }

  public class Tracker
  {
    [JsonProperty("abstract")]
    public Metadata[] Metadata { get; set; }
  }

  public class Metadata
  {
    public string Version { get; set; }

    [JsonProperty("TOT")]
    public int Tot { get; set; }

    public string Indicatif { get; set; }
  }
}