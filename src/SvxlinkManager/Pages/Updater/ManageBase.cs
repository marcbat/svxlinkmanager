using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using SvxlinkManager.Exceptions;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
  ///[Authorize]
  public class ManageBase : SvxlinkManagerComponentBase, IDisposable
  {
    protected override async Task OnInitializedAsync()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Updater page") { Url = new Uri("/Updater/Manage", UriKind.Relative) });

      await base.OnInitializedAsync().ConfigureAwait(false);

      UpdaterService.OnDownloadProgress += UpdaterService_OnDownloadProgressAsync;

      UpdaterService.OnDownloadStart += UpdaterService_OnDownloadStart;

      UpdaterService.OndownloadComplete += UpdaterService_OndownloadComplete;

      UpdaterService.OnReleasesDownloadCompleted += UpdaterService_OnReleasesDownloadCompleted;

      try
      {
        UpdaterService.LoadReleases();
      }
      catch (Exception e)
      {
        Logger.LogError("Impossible de charger la liste des releases.", e);
        Telemetry.TrackException(e);

        await ShowErrorToastAsync("Release", $"Impossible d'otenir la liste des release.");
      }
    }

    private async void UpdaterService_OnReleasesDownloadCompleted()
    {
      await ShowSuccessToastAsync("Release", $"Les releases ont bien été téléchargée.");
      StateHasChanged();
    }

    private async void UpdaterService_OndownloadComplete(Release release)
    {
      await ShowSuccessToastAsync("Mise à jour", $"La version {release.TagName} est téléchargée.");
      StateHasChanged();
    }

    private async void UpdaterService_OnDownloadStart(Release release)
    {
      await ShowInfoToastAsync("Mise à jour", $"La version {release.TagName} est en cours de téléchargement.");
      await Js.InvokeVoidAsync("DownloadUpdate", release.Id);
    }

    private async void UpdaterService_OnDownloadProgressAsync((int releaseId, int progressPercentage) status)
    {
      await Js.InvokeVoidAsync("UpdateDownloadStatus", status.releaseId, status.progressPercentage);
    }

    [Inject]
    public UpdaterService UpdaterService { get; set; }

    public bool IsUpToDate() => UpdaterService.IsUpToDate();

    public async Task InstallAsync(Release release)
    {
      await Js.InvokeVoidAsync("UpdateInstallStatus", release.Id, "Installation en cours");

      try
      {
        UpdaterService.Install(release);
      }
      catch (Exception e)
      {
        Telemetry.TrackException(e);

        await ShowErrorToastAsync("Erreur", e.Message);
      }

      StateHasChanged();
    }

    public async Task DownloadAsync(Release release)
    {
      try
      {
        UpdaterService.Download(release);
      }
      catch (Exception e)
      {
        Telemetry.TrackException(new UpdateException("Echec du telechargement de la mise à jour.", e), new Dictionary<string, string> { { "Name", release.Name } });

        await ShowErrorToastAsync($"Erreur", $"Echec du telechargement de la mise à jour {release.Package.Name}.<br/> {e.Message}");
      }
    }

    public void Dispose()
    {
      UpdaterService.OnDownloadProgress -= UpdaterService_OnDownloadProgressAsync;

      UpdaterService.OnDownloadStart -= UpdaterService_OnDownloadStart;

      UpdaterService.OndownloadComplete -= UpdaterService_OndownloadComplete;

      UpdaterService.OnReleasesDownloadCompleted -= UpdaterService_OnReleasesDownloadCompleted;
    }
  }
}