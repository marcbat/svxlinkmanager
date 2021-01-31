using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Pages.Shared;
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
    public SvxLinkService SvxLinkService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ISa818Service Sa818Service { get; set; }

    public List<Models.RadioProfile> RadioProfiles { get; set; }

    public async Task DeleteAsync(int id)
    {
      Repositories.RadioProfiles.Delete(id);

      Telemetry.TrackEvent("Delete radio profile", RadioProfiles.Single(c => c.Id == id).TrackProperties);

      RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

      await ShowSuccessToastAsync("Supprimé", "Le profil radio a bien été supprimé.");

      StateHasChanged();
    }

    public async Task ApplyAsync(int id)
    {
      var profile = Repositories.RadioProfiles.Get(id);

      if (profile.HasSa818)
        Sa818Service.WriteRadioProfile(profile);

      profile.Enable = true;
      Repositories.RadioProfiles.Update(profile);

      Telemetry.TrackEvent("Apply radio profile", profile.TrackProperties);

      SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

      await ShowSuccessToastAsync($"{profile.Name} appliqué.", $"Le profil radio {profile.Name} a bien été appliqué.");

      NavigationManager.NavigateTo("/RadioProfile/Manage", true);
    }
  }
}