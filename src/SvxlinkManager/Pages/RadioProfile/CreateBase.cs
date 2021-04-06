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
  public class CreateBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Create Page") { Url = new Uri("/RadioProfile/Create", UriKind.Relative) });

      RadioProfile = new Models.RadioProfile();
    }

    protected override string SubmitTitle => "Créer";

    protected override async Task HandleValidSubmitAsync()
    {
      Repositories.RadioProfiles.Add(RadioProfile);

      Telemetry.TrackEvent("Create radio profile", RadioProfile.TrackProperties);

      await ShowSuccessToastAsync("Crée", $"Le profil radio {RadioProfile.Name} a bien été crée.");

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }
  }
}