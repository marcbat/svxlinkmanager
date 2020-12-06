using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class ManageBase : ChannelBase
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public async Task DeleteAsync(int id)
    {
      Repositories.Channels.Delete(id);

      Channels.Remove(Channels.Single(c => c.Id == id));

      StateHasChanged();

      await ShowSuccessToastAsync("Supprimé", "Le salon a bien été supprimé.");

      NavigationManager.NavigateTo("/Channel/Manage");
    }
  }
}