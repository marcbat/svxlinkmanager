using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands;

using System;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
    [Authorize]
    public class EditBase : AddEditBase
    {
        protected override void OnInitialized()
        {
            Telemetry.TrackPageView(new PageViewTelemetry("Reflector Edit Page") { Url = new Uri("/Reflector/Edit", UriKind.Relative) });

            Reflector = Mediatr.Repository<Models.Reflector>().Get(int.Parse(Id));
        }

        [Parameter]
        public string Id { get; set; }

        override protected async Task HandleValidSubmitAsync()
        {
            await Mediatr.Send(new UpdateReflectorCommand(Guid.NewGuid(), Reflector.Id, Reflector.Name, Reflector.Config));

            Telemetry.TrackEvent("Update reflector", Reflector.TrackProperties);

            await ShowSuccessToastAsync("Modifié", $"le reflecteur {Reflector.Name} a bien été modifié.");

            if (Reflector.Enable)
            {
                SvxLinkService.RestartReflector(Reflector);
                await ShowSuccessToastAsync("Redémarrage", $"le reflecteur {Reflector.Name} a bien été redémarré.");
            }

            NavigationManager.NavigateTo("Reflector/Manage");
        }

        protected override string SubmitTitle => "Modifier";
    }
}