using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;
using SvxlinkManager.Models;

using System;
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

            await base.HandleValidSubmit();

            switch (Channel)
            {
                case SvxlinkChannel svxlinkChannel:
                    await Mediatr.Send(new AddSvxlinkChannelCommand(Options.Value.ConfigId, svxlinkChannel.Name, svxlinkChannel.Host, svxlinkChannel.CallSign, svxlinkChannel.AuthKey, svxlinkChannel.Port, svxlinkChannel.ReportCallSign, svxlinkChannel.Sound.SoundName, svxlinkChannel.Sound.SoundFile), CancellationToken.None);

                    break;

                case EcholinkChannel echolinkChannel:
                    await Mediatr.Send(new AddEcholinkChannelCommand(Options.Value.ConfigId, echolinkChannel.Name, echolinkChannel.Host, echolinkChannel.CallSign, echolinkChannel.Password, echolinkChannel.SysopName, echolinkChannel.Location, echolinkChannel.MaxQso, echolinkChannel.Description, echolinkChannel.Sound.SoundName, echolinkChannel.Sound.SoundFile), CancellationToken.None);

                    break;

                case AdvanceSvxlinkChannel advanceChannel:
                    await Mediatr.Send(new AddAdvanceSvxlinkChannelCommand(Options.Value.ConfigId, advanceChannel.Name, advanceChannel.SvxlinkConf, advanceChannel.ModuleDtmfRepeater, advanceChannel.ModuleEchoLink, advanceChannel.ModuleFrn, advanceChannel.ModuleHelp, advanceChannel.ModuleMetarInfo, advanceChannel.ModuleParrot, advanceChannel.ModulePropagationMonitor, advanceChannel.ModuleSelCallEnc, advanceChannel.ModuleTclVoiceMail, advanceChannel.ModuleTrx, advanceChannel.Sound.SoundName, advanceChannel.Sound.SoundFile), CancellationToken.None);

                    break;

                default:
                    throw new InvalidOperationException("Invalid channel type");
            }

            await ShowSuccessToastAsync("Success", $"Le salon {Channel.Name} a été crée.");

            NavigationManager.NavigateTo($"{redirect}/Manage");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Channel = new TChannel
            {
                Sound = new Sound()
            };
        }
    }
}