using Microsoft.AspNetCore.Components;
using Spotnik.Gui.Models;

namespace Spotnik.Gui.Pages
{
  public class AddChannelBase : AddEditChannelBase
  {

    protected override void OnInitialized()
    {
      Channel = new Channel();
    }

    protected void HandleValidSubmit()
    {
      Repositories.Channels.Add(Channel);

      NavigationManager.NavigateTo("channelList");
    }
  }
}
