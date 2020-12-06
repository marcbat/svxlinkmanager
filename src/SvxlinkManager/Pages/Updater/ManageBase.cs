using Microsoft.JSInterop;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Updater
{
  public class ManageBase : SvxlinkManagerComponentBase
  {
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

    public List<Release> Releases { get; set; }

    public string CurrentVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public bool IncludePrerelease { get; set; } = false;

    public bool IsExist(Release release) => File.Exists($"/tmp/svxlinkmanager/{release.Assets.SingleOrDefault(a => a.Name.Contains("updater"))?.Name}");

    public bool IsCurrent(Release release) => release.TagName == CurrentVersion;

    public void Install(Release release)
    {
    }

    public async Task DownloadAsync(Release release)
    {
      var package = release.Assets.Single(a => a.Name.Contains("svxlinkmanager"));
      var updater = release.Assets.Single(a => a.Name.Contains("updater"));

      Directory.CreateDirectory("/tmp/svxlinkmanager");

      using WebClient client = new WebClient();
      client.Headers.Add(HttpRequestHeader.UserAgent, "request");

      client.DownloadProgressChanged += async (s, e) =>
      {
        if ((string)e.UserState == package.Name)
          await Js.InvokeVoidAsync("UpdateDownloadStatus", release.Id, e.ProgressPercentage);
      };

      client.DownloadFileCompleted += async (s, e) =>
      {
        if ((string)e.UserState == package.Name)
        {
          await ShowSuccessToastAsync("Mise à jour", $"La version {release.TagName} est téléchargée.");

          client.DownloadFileAsync(new Uri(updater.DownloadUrl), $"/tmp/svxlinkmanager/{updater.Name}", updater.Name);
        }

        if ((string)e.UserState == updater.Name)
          StateHasChanged();
      };

      await ShowInfoToastAsync("Mise à jour", $"La version {release.TagName} est en cours de téléchargent.");
      await Js.InvokeVoidAsync("DownloadUpdate", release.Id);
      client.DownloadFileAsync(new Uri(package.DownloadUrl), $"/tmp/svxlinkmanager/{package.Name}", package.Name);
    }
  }

  public class Release
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonIgnore]
    public string Created => DateTime.Parse(CreatedAt).ToString("dd MMMM yyyy HH:mm");

    [JsonPropertyName("assets_url")]
    public string AssetsUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; }
  }

  public class Asset
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string DownloadUrl { get; set; }
  }
}