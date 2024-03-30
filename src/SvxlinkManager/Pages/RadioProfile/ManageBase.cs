using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
    [Authorize]
    public class ManageBase : MediatrComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Manage Page") { Url = new Uri("/RadioProfile/Manage", UriKind.Relative) });

            await base.OnInitializedAsync().ConfigureAwait(false);

            LoadRadioProfiles();
        }

        private void LoadRadioProfiles() => RadioProfiles = Mediatr.RadioProfiles.GetAll().ToList();

        [Inject]
        public SvxLinkService SvxLinkService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISa818Service Sa818Service { get; set; }

        public List<Models.RadioProfile> RadioProfiles { get; set; }

        public async Task DeleteAsync(Guid id)
        {
            await Mediatr.Send(new DeleteRadioProfilCommand(Guid.NewGuid(), id));

            Telemetry.TrackEvent("Delete radio profile", RadioProfiles.Single(c => c.Id == id).TrackProperties);

            RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

            await ShowSuccessToastAsync("Supprimé", "Le profil radio a bien été supprimé.");

            StateHasChanged();
        }

        public async Task ApplyAsync(Guid id)
        {
            var profile = Mediatr.RadioProfiles.Get(id);

            if (profile.HasSa818)
                Sa818Service.WriteRadioProfile(profile);

            profile.Enable = true;

            await Mediatr.Send(new UpdateRadioProfilCommand(Guid.NewGuid(), profile.Id, profile.Name, profile.RxFequ, profile.TxFrequ, profile.Squelch, profile.TxCtcss, profile.RxCtcss, profile.Volume, profile.PreEmph, profile.HightPass, profile.LowPass, profile.SquelchDetection));

            Telemetry.TrackEvent("Apply radio profile", profile.TrackProperties);

            if (SvxLinkService.ChannelId > 0)
                SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

            await ShowSuccessToastAsync($"{profile.Name} appliqué.", $"Le profil radio {profile.Name} a bien été appliqué.");

            NavigationManager.NavigateTo("/RadioProfile/Manage", true);
        }
    }
}