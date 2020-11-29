using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

    [Inject]
    public ISa818Service Sa818Service { get; set; }

    public List<Models.RadioProfile> RadioProfiles { get; set; }

    public void Delete(int id)
    {
      Repositories.RadioProfiles.Delete(id);

      RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

      StateHasChanged();
    }

    public void Apply(int id)
    {
      var profile = Repositories.RadioProfiles.Get(id);

      Sa818Service.WriteRadioProfile(profile);

      profile.Enable = true;
      Repositories.RadioProfiles.Update(profile);

      NavigationManager.NavigateTo("/RadioProfile/Manage", true);
    }

    private void ShellOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      Logger.LogInformation(e.Data);
    }

    private void ShellErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      Logger.LogInformation(e.Data);
    }
  }
}