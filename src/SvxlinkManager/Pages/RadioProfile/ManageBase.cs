using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public class ManageBase : RepositoryComponentBase
  {

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadRadioProfiles();
    }

    private void LoadRadioProfiles() => RadioProfiles = Repositories.RadioProfiles.GetAll().ToList();

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<Models.RadioProfile> RadioProfiles { get; set; }

    public void Delete(int id)
    {
      Repositories.Channels.Delete(id);

      RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

      StateHasChanged();

      NavigationManager.NavigateTo("/RadioProfile/Manage");
    }
  }
}
