using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  [Authorize]
  public class EditBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Edit Page") { Url = new Uri("/RadioProfile/Edit", UriKind.Relative) });

      RadioProfile = Mediatr.RadioProfiles.Get(int.Parse(Id));
    }

    [Parameter]
    public string Id { get; set; }

    override protected async Task HandleValidSubmitAsync()
    {
      await Mediatr.Send(new UpdateRadioProfilCommand(Guid.NewGuid(), RadioProfile.Id, RadioProfile.Name, RadioProfile.RxFequ, RadioProfile.TxFrequ, RadioProfile.Squelch, RadioProfile.TxCtcss, RadioProfile.RxCtcss, RadioProfile.Volume, RadioProfile.PreEmph, RadioProfile.HightPass, RadioProfile.LowPass, RadioProfile.SquelchDetection));

      Telemetry.TrackEvent("Update radio profile", RadioProfile.TrackProperties);

      await ShowSuccessToastAsync("Modifié", $"le profil radio {RadioProfile.Name} a bien été modifié.");

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}