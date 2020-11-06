using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public class EditBase : AddEditBase
  {

    protected override void OnInitialized()
    {
      RadioProfile = Repositories.RadioProfiles.Get(int.Parse(Id));
    }

    [Parameter]
    public string Id { get; set; }

    override protected void HandleValidSubmit()
    {
      Repositories.RadioProfiles.Update(RadioProfile);

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}
