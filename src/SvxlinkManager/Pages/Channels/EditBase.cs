using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;
using SvxlinkManager.Models;

using System;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
    [Authorize]
    public class EditBase<TChannel> : AddEditBase<TChannel> where TChannel : ManagedChannel
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public ISvxlinkServiceBase SvxLinkService { get; set; }

        protected override string SubmitTitle => "Modifier";

        /// <summary>
        /// Handles the form submit.
        /// </summary>
        public virtual async Task HandleValidSubmit(string redirect)
        {
            await base.HandleValidSubmit();

            await ShowSuccessToastAsync("Modifié", $"Le salon {Channel.Name} a bien été modifié.");

            NavigationManager.NavigateTo($"{redirect}/Manage");
        }

    }

    public class EditSvxlinkChannelBase : EditBase<SvxlinkChannel>
    {
        protected override async void OnInitialized()
        {
            Channel = await Mediatr.Send(new GetSvxlinkChannelByIdQuery(Guid.NewGuid(), Channel.Id));
        }
    }

    public class EditEcholinkChannelBase : EditBase<EcholinkChannel>
    {
        protected override async void OnInitialized()
        {
            Channel = await Mediatr.Send(new GetEchoLinkChannelByIdQuery(Guid.NewGuid(), Channel.Id));
        }
    }

    public class EditAdvanceSvxlinkChannelBase : EditBase<AdvanceSvxlinkChannel>
    {
        protected override async void OnInitialized()
        {
            Channel = await Mediatr.Send(new GetAvanceSvxlinkChannelByIdQuery(Guid.NewGuid(), Channel.Id));
        }
    }
}