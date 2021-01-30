using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Models;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class CreateBase<TChannel> : AddEditBase<TChannel> where TChannel : Channel, new()
  {
    protected override string SubmitTitle => "Créer";

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit(string redirect)
    {
      Telemetry.TrackEvent("Create channel", new Dictionary<string, string> { { nameof(Channel.Name), Channel.Name } });

      await base.HandleValidSubmit();

      Repositories.Channels.Add(Channel);

      await ShowSuccessToastAsync("Success", $"Le salon {Channel.Name} a été crée.");

      NavigationManager.NavigateTo($"{redirect}/Manage");
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      Channel = new TChannel();
    }
  }
}