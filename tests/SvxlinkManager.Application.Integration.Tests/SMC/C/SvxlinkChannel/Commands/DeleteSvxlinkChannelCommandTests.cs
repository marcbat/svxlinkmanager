using NSubstitute;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.Channles.SvxlinkChannel.Commands
{
    internal class DeleteSvxlinkChannelCommandTests : BaseTest
    {

        [Test]
        public async Task Handle_WhenValid_ShouldDeleteChannel()
        {
            try
            {
                // arrange
                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                var command = new DeleteSvxlinkChannelCommand(configGuid, installChannels[1]);

                // act
                var result = await mediatr.Send(command);

                // assert
                var config = svxlinkManagerConfigRepository.GetConfig(configGuid);

                var calls = GetReceivedCalls();

                await Verify(new
                {
                    result,
                    config,
                    calls
                }, settings);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                File.Delete(db);
            }


        }
    }
}
