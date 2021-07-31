using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
  [Authorize]
  public class ManageBase : RepositoryComponentBase
  {
    public EditContext EditContext;

    protected override async Task OnInitializedAsync()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Scan page") { Url = new Uri("/Reflector/Manage", UriKind.Relative) });

      await base.OnInitializedAsync().ConfigureAwait(false);
    }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    public bool IsChanged { get; set; }

    public bool IsReflectorRunning { get; set; }

    public string ReflectorConfig { get; set; }

    protected async Task Run()
    {
      Telemetry.TrackEvent("Enable Reflector");

      SvxLinkService.RunReflector();

      await ShowSuccessToastAsync("Activé", $"le scan a bien été activé.");
    }

    protected async Task Stop()
    {
      SvxLinkService.StopReflector();

      await ShowSuccessToastAsync("Désactivé", $"le scan a bien été désactivé.");
    }

    protected async Task HandleValidSubmitAsync()
    {
      Telemetry.TrackEvent("Update reflector");

      SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

      await ShowSuccessToastAsync("Modifié", $"La configuration du reflector a bien été modifié.");
    }
  }
}