using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  [Authorize]
  public class ManageBase<TChannel, TLocalizer> : RepositoryComponentBase<TLocalizer> where TChannel : ManagedChannel
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
      switch (default(TChannel))
      {
        case SvxlinkChannel:
          Telemetry.TrackPageView(new PageViewTelemetry("Svxlink Channel Manage Page") { Url = new Uri("/Channel/Manage", UriKind.Relative) });
          break;

        case EcholinkChannel:
          Telemetry.TrackPageView(new PageViewTelemetry("Echolink Channel Manage Page") { Url = new Uri("/Echolink/Manage", UriKind.Relative) });
          break;

        default:
          Telemetry.TrackPageView("Channel Manage Page");
          break;
      }

      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadChannels();
    }

    public List<TChannel> Channels { get; set; }

    private void LoadChannels() => Channels = Repositories.Repository<TChannel>().GetAll().ToList();

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public async Task DeleteAsync(TChannel channel)
    {
      Repositories.Channels.Delete(channel.Id);

      Channels.Remove(channel);

      Telemetry.TrackEvent("Delete channel", channel.TrackProperties);

      StateHasChanged();

      await ShowSuccessToastAsync("Supprimé", "Le salon a bien été supprimé.");
    }
  }
}