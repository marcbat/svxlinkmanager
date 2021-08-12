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
  public class EditBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Reflector Edit Page") { Url = new Uri("/Reflector/Edit", UriKind.Relative) });

      Reflector = Repositories.Repository<Common.Models.Reflector>().Get(int.Parse(Id));
    }

    [Parameter]
    public string Id { get; set; }

    override protected async Task HandleValidSubmitAsync()
    {
      Repositories.Repository<Common.Models.Reflector>().Update(Reflector);

      Telemetry.TrackEvent("Update reflector", Reflector.TrackProperties);

      await ShowSuccessToastAsync("Modifié", $"le reflecteur {Reflector.Name} a bien été modifié.");

      NavigationManager.NavigateTo("Reflector/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}