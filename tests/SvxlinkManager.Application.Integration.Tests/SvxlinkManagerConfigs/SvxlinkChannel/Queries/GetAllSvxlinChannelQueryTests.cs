using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SvxlinkManagerConfigs.SvxlinkChannel.Queries
{
    
    internal class GetAllSvxlinChannelQueryTests :BaseTest
    {
        [Test]
        public async Task Handle_WhenIsValid_ShouldReturnSvxlinkChannels()
        {
            // Arrange
            var install = CreateDefaultConfigAsync();
            ClearReceivedCalls();

            var query = new GetAllSvxlinChannelQuery(configGuid);
            
            // Act
            var result = await mediatr.Send(query);

            // Assert
            var config = svxlinkManagerConfigRepository.GetConfig(configGuid);
            var calls = GetReceivedCalls();
            await Verify(new
            {
                result,
                config,
                calls
            });
        }
    }
}
