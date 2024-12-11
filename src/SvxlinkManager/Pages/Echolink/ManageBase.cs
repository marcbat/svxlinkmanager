using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Channels;

using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Echolink
{
    public class ManageBase : ManageBase<EcholinkChannel>
    {
        protected override async Task LoadChannels()
        {
            var channels = await Mediatr.Send(new GetAllEcholinkChannelsQuery(Options.Value.ConfigId));

            channels.Match(
                Fail: async error => { 
                    
                    await ShowErrorToastAsync("Erreur", error.ToFullString()); 
                },
                Succ: success => Channels = success.Select<Domain.Entities.EcholinkChannel, Models.EcholinkChannel>(c => c).ToList()
            );

        }
    }
}
