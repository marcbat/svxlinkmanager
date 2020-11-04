using Microsoft.AspNetCore.Components;
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

    public void Delete(int id)
    {
      Repositories.Channels.Delete(id);

      Channels.Remove(Channels.Single(c=>c.Id == id));

      StateHasChanged();

      NavigationManager.NavigateTo("channelList");
    }

  }
}
