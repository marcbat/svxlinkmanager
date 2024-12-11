
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries;
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
            await base.OnInitializedAsync().ConfigureAwait(false);

            await LoadRadioProfiles();
        }

        private async Task LoadRadioProfiles()
        {
            var radiosProfil = await Mediatr.Send(new GetAllRadioProfilQuery(Options.Value.ConfigId));

            radiosProfil.Match(Fail: async error => 
                { 
                    await ShowErrorToastAsync("Erreur", error.ToFullString()); 
                }, Succ: success => RadioProfiles = success.Select<Domain.Entities.RadioProfil, Models.RadioProfile>(r => r).ToList()
            );
        }

        [Inject]
        public ISvxlinkServiceBase SvxLinkService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISa818Service Sa818Service { get; set; }

        public List<Models.RadioProfile> RadioProfiles { get; set; }

        public async Task DeleteAsync(Guid id)
        {
            await Mediatr.Send(new DeleteRadioProfilCommand(Options.Value.ConfigId, id));

            RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

            await ShowSuccessToastAsync("Supprimé", "Le profil radio a bien été supprimé.");

            StateHasChanged();
        }

        public async Task ApplyAsync(Guid id)
        {
            await Mediatr.Send(new ApplyRadioProfilCommand(Options.Value.ConfigId, id));

            await ShowSuccessToastAsync($"Profil appliqué.", $"Le profil radio a bien été appliqué.");

            NavigationManager.NavigateTo("/RadioProfile/Manage", true);
        }
    }
}