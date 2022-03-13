using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
    [Authorize]
    public class CreateBase : AddEditBase<CreateBase>
    {
        protected override void OnInitialized()
        {
            Telemetry.TrackPageView(new PageViewTelemetry("Radio Profile Create Page") { Url = new Uri("/Reflector/Create", UriKind.Relative) });

            Reflector = new Domain.Entities.Reflector();

            var sb = new StringBuilder();
            sb.AppendLine("[GLOBAL]");
            sb.AppendLine("TIMESTAMP_FORMAT = \"%c\"");
            sb.AppendLine("LISTEN_PORT = 5300");
            sb.AppendLine("SQL_TIMEOUT = 600");
            sb.AppendLine("SQL_TIMEOUT_BLOCKTIME = 60");
            sb.AppendLine("CODECS = OPUS");
            sb.AppendLine(string.Empty);
            sb.AppendLine("[USERS]");
            sb.AppendLine("SM0ABC-1=MyNodes");
            sb.AppendLine("SM0ABC-2=MyNodes");
            sb.AppendLine("SM1XYZ=SM1XYZ");
            sb.AppendLine(string.Empty);
            sb.AppendLine("[PASSWORDS]");
            sb.AppendLine("MyNodes = \"A very strong password!\"");
            sb.AppendLine("SM1XYZ=\"Another very good password ? \"");

            Reflector.Config = sb.ToString();
        }

        protected override string SubmitTitle => Loc["Create"];

        protected override async Task HandleValidSubmitAsync()
        {
            Repositories.Repository<Domain.Entities.Reflector>().Add(Reflector);

            Telemetry.TrackEvent("Create radio profile", Reflector.TrackProperties);

            await ShowSuccessToastAsync("Crée", $"Le profil radio {Reflector.Name} a bien été crée.");

            NavigationManager.NavigateTo("Reflector/Manage");
        }
    }
}