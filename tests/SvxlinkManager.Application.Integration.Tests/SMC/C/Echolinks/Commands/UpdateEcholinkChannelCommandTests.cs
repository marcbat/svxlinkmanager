using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Commands;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.C.Echolinks.Commands
{
    internal class UpdateEcholinkChannelCommandTests : BaseTest
    {
        [Test]
        public async Task UpdateEcholinkChannelCommand_ShouldUpdateEcholinkChannel()
        {
            try
            {
                // Arrange
                var echolinkChannelGuid = Guid.NewGuid();
                var install = from ___ in CreateDefaultConfig()
                              from conf in svxlinkManagerConfigRepository.GetConfig(configGuid)
                              from echolinkChannel in EcholinkChannel.Create(echolinkChannelGuid, "EcholinkChannelToUpdate", "localhost", "EcholinkCallSign", "1", "My name", "My location", 1, "my description")
                              from _ in conf.AddEcholinkChannel(echolinkChannel)
                              from __ in svxlinkManagerConfigRepository.UpdateAsync(conf)
                              select conf;
                ClearReceivedCalls();

                var command = new UpdateEcholinkChannelCommand(configGuid, echolinkChannelGuid, "EcholinkChannelUpdated", "localhost updated", "EcholinkCallSign updated", "1", "My name updated", "My location updated", 3, "my description updated");
                
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
