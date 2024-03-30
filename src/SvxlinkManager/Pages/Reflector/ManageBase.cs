using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands;
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
            Telemetry.TrackPageView(new PageViewTelemetry("Reflector Manage Page") { Url = new Uri("/Reflector/Manage", UriKind.Relative) });

            await base.OnInitializedAsync().ConfigureAwait(false);

            LoadReflectors();
        }

        private void LoadReflectors() => Reflectors = Mediatr.Reflectors.GetAll().ToList();

        [Inject]
        public SvxLinkService SvxLinkService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISa818Service Sa818Service { get; set; }

        [Inject]
        public IIniService IniService { get; set; }

        public List<Models.Reflector> Reflectors { get; set; }

        public async Task DeleteAsync(Guid id)
        {
            await Mediatr.Send(new DeleteReflectorCommand(Guid.NewGuid(), id));

            Telemetry.TrackEvent("Delete reflector profile", Reflectors.Single(c => c.Id == id).TrackProperties);

            Reflectors.Remove(Reflectors.Single(c => c.Id == id));

            await ShowSuccessToastAsync("Supprimé", "le reflecteur a bien été supprimé.");
        }

        public async Task StartAsync(int id)
        {
            var reflector = Mediatr.Reflectors.Get(id);

            reflector.Enable = true;

            await Mediatr.Send(new UpdateReflectorCommand(Guid.NewGuid(), reflector.Id, reflector.Name, reflector.Config));

            Telemetry.TrackEvent("Start reflector", reflector.TrackProperties);
            SvxLinkService.ActivateReflector(reflector);

            await ShowSuccessToastAsync($"{reflector.Name} démarré.", $"Le reflecteur {reflector.Name} a bien été démarré.");

            Replace(Reflectors, reflector);
        }

        public async Task StopAsync(int id)
        {
            var reflector = Mediatr.Reflectors.Get(id);

            reflector.Enable = false;
            await Mediatr.Send(new UpdateReflectorCommand(Guid.NewGuid(), reflector.Id, reflector.Name, reflector.Config));

            Telemetry.TrackEvent("Stop reflector", reflector.TrackProperties);
            SvxLinkService.StopReflector(reflector);

            await ShowSuccessToastAsync($"{reflector.Name} arreté.", $"Le reflecteur {reflector.Name} a bien été arreté.");

            Replace(Reflectors, reflector);
        }
    }
}