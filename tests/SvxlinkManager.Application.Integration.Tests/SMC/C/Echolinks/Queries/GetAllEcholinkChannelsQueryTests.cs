using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks.Queries;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.C.Echolinks.Queries
{
    internal class GetAllEcholinkChannelsQueryTests : BaseTest
    {
        [Test]
        public async Task GetAllEcholinkChannelsQuery_ShouldGetAllEcholinkChannels()
        {
            try
            {
                // Arrange
                var install = from _ in CreateDefaultConfig()
                              from conf in svxlinkManagerConfigRepository.GetConfig(configGuid)
                              from channel in EcholinkChannel.Create(Guid.NewGuid(), "EcholinkChannel1", "localhost", "EcholinkCallSign", "1", "My name", "My location", 1, "my description")
                              from __ in conf.AddEcholinkChannel(channel)
                              from ___ in svxlinkManagerConfigRepository.UpdateAsync(conf)
                              select conf;
                ClearReceivedCalls();

                var command = new GetAllEcholinkChannelsQuery(configGuid);

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
