using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Exceptions;
using SvxlinkManager.Pages.Updater;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public class UpdaterService
  {
    private readonly IConfiguration configuration;
    private readonly TelemetryClient telemetry;
    private readonly ILogger<UpdaterService> logger;

    private List<Release> releases;

    public event Action OnReleasesDownloadCompleted;

    public event Action<Release> OnDownloadStart;

    public event Action<(int releaseId, int progressPercentage)> OnDownloadProgress;

    public event Action<Release> OndownloadComplete;

    public UpdaterService(IConfiguration configuration, TelemetryClient telemetry, ILogger<UpdaterService> logger)
    {
      this.configuration = configuration;
      this.telemetry = telemetry;
      this.logger = logger;
    }

    public void LoadReleases()
    {
      logger.LogInformation("Chargement de la list des release.");

      try
      {
        using WebClient client = new WebClient();

        client.DownloadStringCompleted += (s, e) =>
        {
          releases = JsonSerializer.Deserialize<List<Release>>(e.Result);
          OnReleasesDownloadCompleted?.Invoke();
        };

        client.Headers.Add(HttpRequestHeader.UserAgent, "request");
        client.DownloadStringAsync(new Uri("https://api.github.com/repos/marcbat/svxlinkmanager/releases"));
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Release GetLastRelease()
    {
      try
      {
        using WebClient client = new WebClient();

        client.Headers.Add(HttpRequestHeader.UserAgent, "request");
        var result = client.DownloadString(new Uri("https://api.github.com/repos/marcbat/svxlinkmanager/releases"));

        return JsonSerializer.Deserialize<List<Release>>(result).First();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<Release> Releases
    {
      get
      {
        if (configuration.GetValue<bool>("Config:IsPreRelease"))
        {
          telemetry.TrackEvent("Chargement des PreRelease.");
          return releases;
        }
        else
          return releases.Where(r => !r.Prerelease).ToList();
      }
    }

    public string CurrentVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public bool IsExist(Release release) => File.Exists($"/tmp/svxlinkmanager/{release.Updater?.Name}");

    public bool IsCurrent(Release release) => release.TagName == CurrentVersion;

    public bool IsUpToDate() => IsCurrent(Releases.First());

    public Release LastRelease => Releases.First();

    public void Install(Release release)
    {
      ExecuteCommand($"chmod 755 /tmp/svxlinkmanager/{release.Updater.Name}");

      var (result, error) = ExecuteCommand($"/tmp/svxlinkmanager/{release.Updater.Name} update");

      if (!string.IsNullOrEmpty(error))
        throw (new UpdateException($"Echec de l'installation de la release. {error}"));
    }

    protected virtual (string, string) ExecuteCommand(string cmd)
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

    public void Download(Release release)
    {
      var releaseUrl = new Uri(release.Package.DownloadUrl);

      var downloadTacker = new DependencyTelemetry()
      {
        Id = Guid.NewGuid().ToString(),
        Name = "DownloadRelease",
        Data = releaseUrl.AbsolutePath,
        Target = releaseUrl.Authority,
        Type = "http"
      };

      var updaterUrl = new Uri(release.Updater.DownloadUrl);

      var downloadUpdaterTacker = new DependencyTelemetry
      {
        Id = Guid.NewGuid().ToString(),

        Name = "DownloadUpdate",
        Data = updaterUrl.AbsolutePath,
        Target = updaterUrl.Authority,
        Type = "http"
      };

      using (var operation = telemetry.StartOperation(downloadTacker))
      {
        telemetry.TrackEvent("Download release file", new Dictionary<string, string> { { "Name", release.Name } });

        var downloadPath = "/tmp/svxlinkmanager";

        try
        {
          if (Directory.Exists(downloadPath))
            Directory.Delete(downloadPath, true);

          var downloadDirectory = Directory.CreateDirectory(downloadPath);

          using WebClient client = new WebClient();
          client.Headers.Add(HttpRequestHeader.UserAgent, "request");

          var packageCheckSum = client.DownloadString(release.PackageCheckSum.DownloadUrl).Split(' ')[0].ToUpper();
          var UpdaterCheckSum = client.DownloadString(release.UpdaterCheckSum.DownloadUrl).Split(' ')[0].ToUpper();

          var packageTarget = $"{downloadDirectory.FullName}/{release.Package.Name}";
          var updaterTarget = $"{downloadDirectory.FullName}/{release.Updater.Name}";

          client.DownloadProgressChanged += (s, e) => OnDownloadProgress?.Invoke((release.Id, e.ProgressPercentage));

          client.DownloadFileCompleted += async (s, e) =>
          {
            logger.LogInformation($"Telechargement de la release {release.Package.DownloadUrl} complet.");

            if (packageCheckSum != GetChecksum(packageTarget))
              throw new Exception($"Echec de la validation du fichier {release.Package.Name}.");

            using (var operation = telemetry.StartOperation(downloadTacker))
            {
              telemetry.TrackEvent("Download Update file", new Dictionary<string, string> { { "Name", release.Name } });
              logger.LogInformation($"Download Update file {release.Updater.DownloadUrl}");

              client.DownloadFile(new Uri(release.Updater.DownloadUrl), updaterTarget);

              if (UpdaterCheckSum != GetChecksum(updaterTarget))
                throw new Exception($"Echec de la validation du fichier {release.Updater.Name}.");

              OndownloadComplete?.Invoke(release);
            }
          };

          logger.LogInformation($"Telechargement de la release {release.Package.DownloadUrl}");
          client.DownloadFileAsync(new Uri(release.Package.DownloadUrl), packageTarget);
          OnDownloadStart?.Invoke(release);
        }
        catch (Exception e)
        {
          Directory.Delete(downloadPath, true);

          throw (new UpdateException("Echec du telechargement de la mise à jour.", e));
        }
      }
    }

    private static string GetChecksum(string file)
    {
      using (var stream = File.OpenRead(file))
      {
        var sha = new SHA256Managed();
        byte[] checksum = sha.ComputeHash(stream);
        return BitConverter.ToString(checksum).Replace("-", "");
      }
    }
  }
}