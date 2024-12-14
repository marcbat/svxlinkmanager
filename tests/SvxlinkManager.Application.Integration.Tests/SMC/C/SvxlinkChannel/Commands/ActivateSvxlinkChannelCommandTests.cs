using LanguageExt;

using NSubstitute;

using SvxlinkManager.Application.SvxlinkManagerConfigs;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.Channles.SvxlinkChannel.Commands
{
    public class ActivateSvxlinkChannelCommandTests : BaseTest
    {
        private readonly string applicationPath = Directory.GetCurrentDirectory();

        [Test]
        public async Task ActivateSvxlinkChannelCommand_WhenIsValid_ShouldActivateSvxlinkChannel()
        {
            try
            {
                // Arrange
                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                _ = svxlinkManagerConfigRepository.GetConfig(configGuid)
                     .Map(config => new ApplyRadioProfilCommand(configGuid, config.RadioProfiles.First().Id))
                     .Map(async x => await mediatr.Send(x));

                // Act
                var command = new ActivateSvxlinkChannelCommand(configGuid, Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"));
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

        [Test]
        public async Task ActivateSvxlinkChannelCommand_WhenIsValid_ShouldWriteSvxlinkConf()
        {
            try
            {
                // Arrange
                var install = CreateDefaultConfig();

                _ = svxlinkManagerConfigRepository.GetConfig(configGuid)
                    .Map(config => new ApplyRadioProfilCommand(configGuid, config.RadioProfiles.First().Id))
                    .Map(async x => await mediatr.Send(x));

                // Act
                var command = new ActivateSvxlinkChannelCommand(configGuid, Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"));
                var result = await mediatr.Send(command);

                // Assert
                await VerifyFile($"{applicationPath}/SvxlinkConfig/svxlink.conf");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                File.Delete(db);
                File.Delete($"{applicationPath}/SvxlinkConfig/svxlink.conf");
            }

        }
    }
}
