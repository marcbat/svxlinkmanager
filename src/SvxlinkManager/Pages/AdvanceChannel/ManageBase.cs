using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.AvanceSvxlinkChannels.Queries;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Channels;

using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.AdvanceChannel
{
    public class ManageBase : ManageBase<AdvanceSvxlinkChannel>
    {
        protected override async Task LoadChannels()
        {
            var channels = await Mediatr.Send(new GetAllAdvanceChannelsQuery(Options.Value.ConfigId));

            channels.Match(
                Fail: async error =>
                {
                    await ShowErrorToastAsync("Erreur", error.ToFullString());
                },
                Succ: success => Channels = success.Select<Domain.Entities.AdvanceSvxlinkChannel, Models.AdvanceSvxlinkChannel>(c => c).ToList()
            );

        }
    }
}
