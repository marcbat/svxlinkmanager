using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
    [Authorize]
    public abstract class ManageBase<TChannel> : MediatrComponentBase where TChannel : ManagedChannel
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await LoadChannels();
        }

        public List<TChannel> Channels { get; set; }

        protected abstract Task LoadChannels();

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task DeleteAsync(TChannel channel)
        {
            await Mediatr.Send(new DeleteManagedChannelCommand(Options.Value.ConfigId, channel.Id));

            Channels.Remove(channel);

            StateHasChanged();

            await ShowSuccessToastAsync("Supprimé", "Le salon a bien été supprimé.");
        }
    }

    public class ManageBase : ManageBase<SvxlinkChannel>
    {
        protected override async Task LoadChannels()
        {
            var channels = await Mediatr.Send(new GetAllSvxlinChannelQuery(Options.Value.ConfigId));

            channels.Match(
                Fail: async error =>
                {
                    await ShowErrorToastAsync("Erreur", error.ToFullString());
                },
                Succ: success => Channels = success.Select<Domain.Entities.SvxlinkChannel, Models.SvxlinkChannel>(c => c).ToList()
            );

        }
    }

}