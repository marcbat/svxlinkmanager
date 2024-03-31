using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Commands;
using SvxlinkManager.Pages.Channels;

using System;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.AdvanceChannel
{
    public class EditBase : EditBase<Models.AdvanceSvxlinkChannel>
    {
        public override async Task HandleValidSubmit(string redirect)
        {
            await Mediatr.Send(new UpdateAdvanceSvxlinkChannelCommand(Guid.NewGuid(), Channel.Id, Channel.Name, Channel.SvxlinkConf, Channel.ModuleDtmfRepeater, Channel.ModuleEchoLink, Channel.ModuleFrn, Channel.ModuleHelp, Channel.ModuleMetarInfo, Channel.ModuleParrot, Channel.ModulePropagationMonitor, Channel.ModuleSelCallEnc, Channel.ModuleTclVoiceMail, Channel.ModuleTrx, Channel.Sound.SoundFile));

            StateHasChanged();

            //var toto = await Js.InvokeAsync<string>("GetEditorValue", new object[] { "SvxlinkConfEditor" });

            //await Js.InvokeVoidAsync("EditorToTextArea");

            await base.HandleValidSubmit(redirect);
        }
    }
}