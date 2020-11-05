using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public class CreateBase : RepositoryComponentBase
  {
    public Models.RadioProfile RadioProfile { get; private set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
      RadioProfile = new Models.RadioProfile();
    }

    protected void HandleValidSubmit()
    {
      Repositories.RadioProfiles.Add(RadioProfile);

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }
  }
}
