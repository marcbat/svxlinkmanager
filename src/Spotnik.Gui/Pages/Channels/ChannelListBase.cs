using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Pages
{
  public class ChannelListBase : ChannelBase
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
