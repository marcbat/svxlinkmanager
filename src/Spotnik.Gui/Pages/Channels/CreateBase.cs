using Microsoft.AspNetCore.Components;
using Spotnik.Gui.Models;

namespace Spotnik.Gui.Pages.Channels
{
  public class CreateBase : AddEditBase
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
