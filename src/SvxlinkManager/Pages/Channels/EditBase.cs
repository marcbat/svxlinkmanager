using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class EditBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      Channel = Repositories.Channels.Get(int.Parse(Id));
    }

    [Parameter]
    public string Id { get; set; }

    public void HandleValidSubmit()
    {
      Repositories.Channels.Update(Channel);

      NavigationManager.NavigateTo("Channel/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}
