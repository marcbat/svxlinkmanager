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
                var soundFile = FileToByteArray(Path.Combine(assemblyPath, "assets", "StarWars3.wav"));

                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                var command = new AddEcholinkChannelCommand(configGuid, "AddedEcholinkChannel", "145.500", "145.800", "1", "67", "88.5", 1, "1", "1", soundFile);

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
