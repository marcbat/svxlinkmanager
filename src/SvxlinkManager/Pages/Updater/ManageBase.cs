using Microsoft.JSInterop;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Updater
{
  public class ManageBase : SvxlinkManagerComponentBase
  {
    private List<Release> releases;
    private bool includePrerelease = false;

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadReleases();
    }

    private void LoadReleases()
    {
      using WebClient client = new WebClient();
      client.Headers.Add(HttpRequestHeader.UserAgent, "request");
      var result = client.DownloadString(new Uri("https://api.github.com/repos/marcbat/svxlinkmanager/releases"));
      Releases = JsonSerializer.Deserialize<List<Release>>(result);
    }

    public List<Release> Releases
    {
      get
      {
        if (IncludePrerelease)
          return releases;
        else
          return releases.Where(r => !r.Prerelease).ToList();
      }
      set => releases = value;
    }

    public string CurrentVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public bool IncludePrerelease
    {
      get => includePrerelease;
      set
      {
        includePrerelease = value;
        StateHasChanged();
      }
    }

    public bool IsExist(Release release) => File.Exists($"/tmp/svxlinkmanager/{release.Updater?.Name}");

    public bool IsCurrent(Release release) => release.TagName == CurrentVersion;

    public async Task InstallAsync(Release release)
    {
      await Js.InvokeVoidAsync("UpdateInstallStatus", release.Id, "Installation en cours");

      ExecuteCommand($"chmod 755 /tmp/svxlinkmanager/{release.Updater.Name}");

      var (result, error) = ExecuteCommand($"/tmp/svxlinkmanager/{release.Updater.Name} update");

      if (!string.IsNullOrEmpty(error))
        await ShowErrorToastAsync("Erreur", error);

      StateHasChanged();
    }

    public async Task DownloadAsync(Release release)
    {
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

        client.DownloadProgressChanged += async (s, e) =>
            await Js.InvokeVoidAsync("UpdateDownloadStatus", release.Id, e.ProgressPercentage);

        client.DownloadFileCompleted += async (s, e) =>
        {
          await ShowSuccessToastAsync("Mise à jour", $"La version {release.TagName} est téléchargée.");

          if (packageCheckSum != GetChecksum(packageTarget))
            throw new Exception($"Echec de la validation du fichier {release.Package.Name}.");

          client.DownloadFile(new Uri(release.Updater.DownloadUrl), updaterTarget);

          if (UpdaterCheckSum != GetChecksum(updaterTarget))
            throw new Exception($"Echec de la validation du fichier {release.Updater.Name}.");

          StateHasChanged();
        };

        await ShowInfoToastAsync("Mise à jour", $"La version {release.TagName} est en cours de téléchargement.");
        await Js.InvokeVoidAsync("DownloadUpdate", release.Id);
        client.DownloadFileAsync(new Uri(release.Package.DownloadUrl), packageTarget);
      }
      catch (Exception e)
      {
        await ShowErrorToastAsync($"Erreur", $"Echec de la mise à jour {release.Package.Name}.<br/> {e.Message}");

        Directory.Delete(downloadPath, true);

        StateHasChanged();
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