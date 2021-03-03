using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class EditBase<TChannel> : AddEditBase<TChannel> where TChannel : Channel
  {
    [Parameter]
    public string Id { get; set; }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    protected override string SubmitTitle => "Modifier";

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit(string redirect)
    {
      await base.HandleValidSubmit();

      Repositories.Channels.Update(Channel);

      if (SvxLinkService.ChannelId == Channel.Id)
        SvxLinkService.ActivateChannel(Channel.Id);

      await ShowSuccessToastAsync("Modifié", $"Le salon {Channel.Name} a bien été modifié.");

      NavigationManager.NavigateTo($"{redirect}/Manage");
    }

    protected override void OnInitialized()
    {
      switch (Channel)
      {
        case SvxlinkChannel channel:
          Telemetry.TrackPageView(new PageViewTelemetry("Svxlink Channel Edit Page") { Url = new Uri("/Channel/Edit", UriKind.Relative) });
          break;

        case EcholinkChannel channel:
          Telemetry.TrackPageView(new PageViewTelemetry("Echolink Channel Edit Page") { Url = new Uri("/Echolink/Edit", UriKind.Relative) });
          break;

        default:
          Telemetry.TrackPageView("Channel Edit Page");
          break;
      }

      Channel = (TChannel)Repositories.Channels.GetWithSound(int.Parse(Id));
    }
  }
}