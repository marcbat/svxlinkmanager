using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Queries;

using System;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
    [Authorize]
    public class EditBase : AddEditBase
    {
        protected override async void OnInitialized()
        {
            Reflector = await Mediatr.Send(new GetReflectorByIdQuery(Guid.NewGuid(), Guid.Parse(Id)));
        }

        [Parameter]
        public string Id { get; set; }

        override protected async Task HandleValidSubmitAsync()
        {
            await Mediatr.Send(new UpdateReflectorCommand(Options.Value.ConfigId, Reflector.Id, Reflector.Name, Reflector.Config));

            await ShowSuccessToastAsync("Modifié", $"le reflecteur {Reflector.Name} a bien été modifié.");

            NavigationManager.NavigateTo("Reflector/Manage");
        }

        protected override string SubmitTitle => "Modifier";
    }
}