using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SvxlinkManagerConfigs.SvxlinkChannel.Queries
{
    internal class GetSvxlinkChannelByIdQueryTests : BaseTest
    {
        [Test]
        public async Task Handle_WhenIsValid_ShouldReturnSvxlinkChannel()
        {
            // Arrange
            var install = CreateDefaultConfigAsync();
            ClearReceivedCalls();
            var query = new GetSvxlinkChannelByIdQuery(configGuid, installChannels.First());

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
            }, settings);
        }
    }
}
