using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests
{
    public class StartDefaultChannelCommandTests :BaseTest
    {
        [Test]
        public async Task StartDefaultChannelCommand_WhenIsValid_StartDefaultChannel()
        {
            // arrange
            var install = CreateDefaultConfig();

            // act
            var result = svxlinkManagerConfigRepository.GetConfig(configGuid)
                .Map(config => new StartDefaultChannelCommand(configGuid))
                .Map(async x => await mediatr.Send(x));

            // assert
            await Verify(result);
        }
    }
}
