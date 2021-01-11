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
      Channel = Repositories.Repository<TChannel>().Get(int.Parse(Id));
    }
  }
}