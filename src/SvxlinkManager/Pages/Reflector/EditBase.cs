﻿using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
    [Authorize]
    public class EditBase : AddEditBase<EditBase>
    {
        protected override void OnInitialized()
        {
            Telemetry.TrackPageView(new PageViewTelemetry("Reflector Edit Page") { Url = new Uri("/Reflector/Edit", UriKind.Relative) });

            Reflector = Repositories.Repository<Domain.Entities.Reflector>().Get(int.Parse(Id));
        }

        [Parameter]
        public string Id { get; set; }

        protected override async Task HandleValidSubmitAsync()
        {
            Repositories.Repository<Domain.Entities.Reflector>().Update(Reflector);

            Telemetry.TrackEvent("Update reflector", Reflector.TrackProperties);

            await ShowSuccessToastAsync("Modifié", $"le reflecteur {Reflector.Name} a bien été modifié.");

            if (Reflector.Enable)
            {
                SvxLinkService.RestartReflector(Reflector);
                await ShowSuccessToastAsync("Redémarrage", $"le reflecteur {Reflector.Name} a bien été redémarré.");
            }

            NavigationManager.NavigateTo("Reflector/Manage");
        }

        protected override string SubmitTitle => Loc["Modify"];
    }
}