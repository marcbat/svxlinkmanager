using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;

using System;
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
            await Mediatr.Send(new AddRadioProfilCommand(Guid.NewGuid(), RadioProfile.Name, RadioProfile.RxFequ, RadioProfile.TxFrequ, RadioProfile.Squelch, RadioProfile.TxCtcss, RadioProfile.RxCtcss, RadioProfile.Volume, RadioProfile.PreEmph, RadioProfile.HightPass, RadioProfile.LowPass, RadioProfile.SquelchDetection));

            Telemetry.TrackEvent("Create radio profile", RadioProfile.TrackProperties);

            await ShowSuccessToastAsync("Crée", $"Le profil radio {RadioProfile.Name} a bien été crée.");

            NavigationManager.NavigateTo("RadioProfile/Manage");
        }
    }
}