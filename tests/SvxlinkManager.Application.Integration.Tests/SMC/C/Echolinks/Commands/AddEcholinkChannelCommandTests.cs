using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.C.Echolinks.Commands
{
    internal class AddEcholinkChannelCommandTests : BaseTest
    {

        [Test]
        public async Task AddEcholinkChannelCommand_ShouldAddEcholinkChannel()
        {
            try
            {
                // Arrange
                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                var command = new AddEcholinkChannelCommand(configGuid, "EcholinkChannel", "localhost", "EcholinkCallSign", "1", "My name", "My location", 1, "my description");

                // Act
                var result = await mediatr.Send(command);

                // Assert
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
