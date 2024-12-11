using NSubstitute;

using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SvxlinkManagerConfigs.SvxlinkChannel.Commands
{
    internal class UpdateSvxlinkChannelCommandTests : BaseTest
    {
        [Test]
        public async Task Handle_WhenIsValid_ShouldUpdateChannel()
        {
            try
            {

                // arrange
                var soundFile = FileToByteArray(Path.Combine(assemblyPath, "assets", "StarWars3.wav"));
                var install = await CreateDefaultConfigAsync();

                var command = new UpdateSvxlinkChannelCommand(configGuid, installChannels.First(), "TestUpdate", "localhostUpdate", "CallsignUpdate", "AuthUpdated", 5050, "ReportCallUpdated", "SoudNameUpdated", soundFile);

                // act
                var result = await mediatr.Send(command);

                // assert
                var config = svxlinkManagerConfigRepository.GetConfig(configGuid);

                var calls = svxlinkService.ReceivedCalls()
                    .Select(x => new { name = x.GetMethodInfo().Name, sequence = x.GetSequenceNumber(), arguments = x.GetArguments() })
                    .OrderBy(x => x.sequence)
                    .Select(x => new { x.name, x.arguments });

                await Verify(new
                {
                    result,
                    config,
                    calls
                });

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
