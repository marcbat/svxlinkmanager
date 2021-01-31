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

    override protected async Task HandleValidSubmitAsync()
    {
      Repositories.RadioProfiles.Update(RadioProfile);

      Telemetry.TrackEvent("Update radio profile", RadioProfile.TrackProperties);

      await ShowSuccessToastAsync("Modifié", $"le profil radio {RadioProfile.Name} a bien été modifié.");

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}