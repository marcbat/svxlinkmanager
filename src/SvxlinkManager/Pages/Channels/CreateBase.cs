using Microsoft.AspNetCore.Components;
using SvxlinkManager.Models;

namespace SvxlinkManager.Pages.Channels
{
  public class CreateBase : AddEditBase
  {
    
    protected override void OnInitialized()
    {
      Channel = new Channel();
    }

    override protected void HandleValidSubmit()
    {
      Repositories.Channels.Add(Channel);

      NavigationManager.NavigateTo("Channel/Manage");
    }

    protected override string SubmitTitle => "Créer";
  }
}
