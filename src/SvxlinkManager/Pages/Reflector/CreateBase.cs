using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
  [Authorize]
  public class CreateBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Create Page") { Url = new Uri("/Reflector/Create", UriKind.Relative) });

      Reflector = new Models.Reflector();
    }

    protected override string SubmitTitle => "Créer";

    protected override async Task HandleValidSubmitAsync()
    {
      Repositories.Repository<Models.Reflector>().Add(Reflector);

      Telemetry.TrackEvent("Create radio profile", Reflector.TrackProperties);

      await ShowSuccessToastAsync("Crée", $"Le profil radio {Reflector.Name} a bien été crée.");

      NavigationManager.NavigateTo("Reflector/Manage");
    }
  }
}