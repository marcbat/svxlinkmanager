using NSubstitute;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SvxlinkManagerConfigs.SvxlinkChannel.Commands
{
    internal class AddSvxlinkChannelCommandTests: BaseTest
    {
        [Test]
        public async Task Handle_WhenValid_ShouldAddSvxlinkChannel()
        {
            try
            {
                // arrange
                var soundFile = FileToByteArray(Path.Combine(assemblyPath, "assets", "StarWars3.wav"));

                var install = await CreateDefaultConfigAsync();
                ClearReceivedCalls();

                var command = new AddSvxlinkChannelCommand(configGuid, "Test", "localhost", "Test", "Test", 8080, "Test", "Test", soundFile);
                
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
