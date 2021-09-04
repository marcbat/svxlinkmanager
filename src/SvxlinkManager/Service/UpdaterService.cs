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
using System.Linq.Expressions;
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

    public event Action OnReleasesDownloadCompleted;

    public event Action<Release> OnDownloadStart;

    public event Action<(int releaseId, int progressPercentage)> OnDownloadProgress;

    public event Action<Release> OndownloadComplete;

    public UpdaterService(IConfiguration configuration, TelemetryClient telemetry, ILogger<UpdaterService> logger)
    {
      this.configuration = configuration;
      this.telemetry = telemetry;
      this.logger = logger;

      Releases = new List<Release>();
    }

    /// <summary>
    /// Load all releases from github
    /// </summary>
    public void LoadReleases()
    {
      logger.LogInformation("Chargement de la list des release.");

      try
      {
        using var client = new WebClient();

        client.DownloadStringCompleted += (s, e) =>
        {
          Releases.AddRange(JsonSerializer.Deserialize<List<Release>>(e.Result).ToList());
          OnReleasesDownloadCompleted?.Invoke();
        };

        Releases.Clear();
        client.Headers.Add(HttpRequestHeader.UserAgent, "request");
        client.DownloadStringAsync(new Uri("https://api.github.com/repos/marcbat/svxlinkmanager/releases"));
      }
      catch (Exception e)
      {
        telemetry.TrackException(new Exception("Impossible de charger les releases", e));
      }
    }

    /// <summary>
    /// List of release
    /// </summary>
    public List<Release> Releases { get; private set; }

    /// <summary>
    /// True if appsetting config parameter IsPreRelease equal true
    /// </summary>
    public bool IsPreRelease => configuration.GetValue<bool>("Config:IsPreRelease");

    /// <summary>
    /// Return current value from the assembly
    /// </summary>
    public string CurrentVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    /// <summary>
    /// Get the major of the current version
    /// </summary>
    public int CurrentMajor => int.Parse(CurrentVersion.Split('.').First());

    /// <summary>
    /// Return last release
    /// </summary>
    /// <returns></returns>
    public Release GetLastRelease() => IsPreRelease ? Releases.First() : Releases.Where(IsStable).Where(IsSameMajor).First();

    /// <summary>
    /// Return last image not in the same major
    /// </summary>
    /// <returns>Return last image or null</returns>
    public Release GetLastImage() => Releases.Where(r => !IsSameMajor(r)).FirstOrDefault(r => r.Image != null);

    /// <summary>
    /// check if release is prerelease
    /// </summary>
    /// <param name="release">the release</param>
    /// <returns>true if is a prerelease</returns>
    public bool IsStable(Release release) => !release.Prerelease;

    /// <summary>
    /// Check if release is in current major range
    /// </summary>
    /// <param name="release">the release</param>
    /// <returns>true if release is in current major release</returns>
    public bool IsSameMajor(Release release) => release.Major == CurrentMajor;

    /// <summary>
    /// Check if a release is already downloaded
    /// </summary>
    /// <param name="release">the release</param>
    /// <returns>true if is dowloaded</returns>
    public bool IsExist(Release release) => File.Exists($"/tmp/svxlinkmanager/{release.Updater?.Name}");

    /// <summary>
    /// Check if the release is the durrent installed release
    /// </summary>
    /// <param name="release">the release</param>
    /// <returns>True if the release is the durrent installed release</returns>
    public bool IsCurrent(Release release) => release.TagName == CurrentVersion;

    /// <summary>
    /// Check if svxlinkmanager is up to date
    /// </summary>
    /// <returns>True if svxlinkmanager is up to date</returns>
    public bool IsUpToDate() => IsCurrent(GetLastRelease());

    /// <summary>
    /// Run the updater sh script
    /// </summary>
    /// <param name="release">the release to install</param>
    public void Install(Release release)
    {
      ExecuteCommand($"chmod 755 /tmp/svxlinkmanager/{release.Updater.Name}");

      var (result, error) = ExecuteCommand($"/tmp/svxlinkmanager/{release.Updater.Name} update");

      if (!string.IsNullOrEmpty(error))
        throw (new UpdateException($"Echec de l'installation de la release. {error}"));
    }

    /// <summary>
    /// Run a bash command
    /// </summary>
    /// <param name="cmd">the command to execute</param>
    /// <returns>result and errors</returns>
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

    /// <summary>
    /// Download the selected release and updated sh script
    /// </summary>
    /// <param name="release">the release</param>
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
                throw new Exception($"Echec de la validation du fichier {release.Updater.Name}. ");

              logger.LogInformation($"Download Update file {release.Updater.DownloadUrl} complet.");

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

    /// <summary>
    /// Get checksum for a specific file
    /// </summary>
    /// <param name="file">the file</param>
    /// <returns>a checksum for file</returns>
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