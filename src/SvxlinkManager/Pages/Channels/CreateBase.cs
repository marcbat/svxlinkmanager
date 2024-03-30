using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Application.SvxlinkManagerConfigs.AvanceSvxlinkChannels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Echolinks.Commands;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  [Authorize]
    public class CreateBase<TChannel> : AddEditBase<TChannel> where TChannel : ManagedChannel, new()
    {
        protected override string SubmitTitle => "Créer";

        /// <summary>
        /// Handles the form submit.
        /// </summary>
        public async Task HandleValidSubmit(string redirect)
        {
            Telemetry.TrackEvent("Create channel", Channel.TrackProperties);

            await base.HandleValidSubmit();

            switch (Channel)
            {
                case SvxlinkChannel svxlinkChannel:
                    await Mediatr.Send(new AddSvxlinkChannelCommand(Guid.NewGuid(), svxlinkChannel.Name, svxlinkChannel.Host, svxlinkChannel.CallSign, svxlinkChannel.Port, svxlinkChannel.ReportCallSign, svxlinkChannel.Sound.SoundFile), CancellationToken.None);
                    Telemetry.TrackPageView(new PageViewTelemetry("Svxlink Channel Create Page") { Url = new Uri("/Channel/Create", UriKind.Relative) });
                    break;

                case EcholinkChannel echolinkChannel:
                    await Mediatr.Send(new AddEcholinkChannelCommand(Guid.NewGuid(), echolinkChannel.Name, echolinkChannel.Host, echolinkChannel.CallSign, echolinkChannel.Password, echolinkChannel.SysopName, echolinkChannel.Location, echolinkChannel.MaxQso, echolinkChannel.Description, echolinkChannel.Sound.SoundFile), CancellationToken.None);
                    Telemetry.TrackPageView(new PageViewTelemetry("Echolink Channel Create Page") { Url = new Uri("/Echolink/Create", UriKind.Relative) });
                    break;

                case AdvanceSvxlinkChannel advanceChannel:
                    await Mediatr.Send(new AddAdvanceSvxlinkChannelCommand(Guid.NewGuid(), advanceChannel.Name, advanceChannel.SvxlinkConf, advanceChannel.ModuleDtmfRepeater, advanceChannel.ModuleEchoLink, advanceChannel.ModuleFrn, advanceChannel.ModuleHelp, advanceChannel.ModuleMetarInfo, advanceChannel.ModuleParrot, advanceChannel.ModulePropagationMonitor, advanceChannel.ModuleSelCallEnc, advanceChannel.ModuleTclVoiceMail, advanceChannel.ModuleTrx, advanceChannel.Sound.SoundFile), CancellationToken.None);
                    Telemetry.TrackPageView("Channel Edit Page");
                    break;

                default:
                    throw new InvalidOperationException("Invalid channel type");
            }

            await ShowSuccessToastAsync("Success", $"Le salon {Channel.Name} a été crée.");

            NavigationManager.NavigateTo($"{redirect}/Manage");
        }

        protected override void OnInitialized()
        {
            switch (Channel)
            {
                case SvxlinkChannel channel:
                    Telemetry.TrackPageView(new PageViewTelemetry("Svxlink Channel Create Page") { Url = new Uri("/Channel/Create", UriKind.Relative) });
                    break;

                case EcholinkChannel channel:
                    Telemetry.TrackPageView(new PageViewTelemetry("Echolink Channel Create Page") { Url = new Uri("/Echolink/Create", UriKind.Relative) });
                    break;

                default:
                    Telemetry.TrackPageView("Channel Edit Page");
                    break;
            }

            base.OnInitialized();

            Channel = new TChannel
            {
                Sound = new Sound()
            };
        }
    }
}