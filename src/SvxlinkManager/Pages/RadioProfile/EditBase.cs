using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  [Authorize]
  public class EditBase : AddEditBase<EditBase>
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Edit Page") { Url = new Uri("/RadioProfile/Edit", UriKind.Relative) });

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