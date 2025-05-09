using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.C.Echolinks.Commands
{
    internal class DeleteEcholinkChannelCommandTests : BaseTest
    {
        [Test]
        public async Task DeleteEcholinkChannelCommand_ShouldDeleteEcholinkChannel()
        {
            try
            {
                // Arrange
                var echolinkChannelGuid = Guid.NewGuid();

                var install = from ___ in CreateDefaultConfig()
                              from conf in svxlinkManagerConfigRepository.GetConfig(configGuid)
                              from echolinkChannel in EcholinkChannel.Create(echolinkChannelGuid, "EcholinkChannelToDelete", "145.500", "145.800", "1", "67", "88.5", 1, "1")
                              from _ in conf.AddEcholinkChannel(echolinkChannel)
                              from __ in svxlinkManagerConfigRepository.UpdateAsync(conf)
                              select conf;

                ClearReceivedCalls();

                var command = new DeleteEcholinkChannelCommand(configGuid, echolinkChannelGuid);

                // Act
                var result = await mediatr.Send(command);

                // Assert
                var config = svxlinkManagerConfigRepository.GetConfig(configGuid);
                var calls = GetReceivedCalls();

                await Verify(new
                {
                    install,
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
