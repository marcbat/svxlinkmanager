
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Queries;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
    [Authorize]
    public class ManageBase : MediatrComponentBase
    {
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync().ConfigureAwait(false);

            await LoadReflectorsAsync();
        }

        private async Task LoadReflectorsAsync()
        {
            var reflectors = await Mediatr.Send(new GetAllReflectorQuery(Options.Value.ConfigId));

            Reflectors =  reflectors.Select<Domain.Entities.Reflector, Models.Reflector>(r => r).ToList();
        }

        [Inject]
        public ISvxlinkServiceBase SvxLinkService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISa818Service Sa818Service { get; set; }

        [Inject]
        public IIniService IniService { get; set; }

        public List<Models.Reflector> Reflectors { get; set; }

        public async Task DeleteAsync(Guid id)
        {
            await Mediatr.Send(new DeleteReflectorCommand(Options.Value.ConfigId, id));

            Reflectors.Remove(Reflectors.Single(c => c.Id == id));

            await ShowSuccessToastAsync("Supprimé", "le reflecteur a bien été supprimé.");
        }

        public async Task StartAsync(Guid id)
        {
            await Mediatr.Send(new StartReflectorCommand(Options.Value.ConfigId, id));

            await ShowSuccessToastAsync($"Reflector démarré.", $"Le reflecteur a bien été démarré.");

            //Replace(Reflectors, reflector);
        }

        public async Task StopAsync(Guid id)
        {
            await Mediatr.Send(new StopReflectorCommand(Options.Value.ConfigId, id));

            await ShowSuccessToastAsync($"Reflector arreté.", $"Le reflecteur a bien été arreté.");

            //Replace(Reflectors, reflector);
        }
    }
}