using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  [Authorize]
  public class CreateBase<TChannel, TLocalizer> : AddEditBase<TChannel, TLocalizer> where TChannel : ManagedChannel, new()
  {
    protected override string SubmitTitle => Loc["Create"];

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit(string redirect)
    {
      Telemetry.TrackEvent("Create channel", Channel.TrackProperties);

      await base.HandleValidSubmit();

      Repositories.Channels.Add(Channel);

      await ShowSuccessToastAsync("Success", $"Le salon {Channel.Name} a été crée.");

      NavigationManager.NavigateTo($"{redirect}/Manage");
    }

    protected override void OnInitialized()
    {
      switch (Channel)
      {
        case SvxlinkChannel channel:
          Telemetry.TrackPageView(new PageViewTelemetry("Svxlink Channel Create Page") { Url = new Uri("/Channel/Create", UriKind.Relative) });
          break;

        case EcholinkChannel channel:
          Telemetry.TrackPageView(new PageViewTelemetry("Echolink Channel Create Page") { Url = new Uri("/Echolink/Create", UriKind.Relative) });
          break;

        default:
          Telemetry.TrackPageView("Channel Edit Page");
          break;
      }

      base.OnInitialized();

      Channel = new TChannel
      {
        Sound = new Sound()
      };
    }
  }
}