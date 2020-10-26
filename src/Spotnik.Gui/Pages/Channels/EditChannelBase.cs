using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Pages
{
  public class EditChannelBase : AddEditChannelBase
  {
    protected override void OnInitialized()
    {
      Channel = Repositories.Channels.Get(int.Parse(Id));
    }

    [Parameter]
    public string Id { get; set; }

    protected void HandleValidSubmit()
    {
      Repositories.Channels.Update(Channel);

      NavigationManager.NavigateTo("channelList");
    }
  }
}
