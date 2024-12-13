using NSubstitute;

using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.Integration.Tests.SvxlinkManagerConfigs.Installer
{
    public class InstallCommandTests : BaseTest
    {
        [Test]
        public async Task InstallCommand_WhenValid_ShouldInstall()
        {
            try
            {
                // arrange
                var install = await CreateDefaultConfigAsync();

                // assert
                var config = svxlinkManagerConfigRepository.GetConfig(configGuid);

                IEnumerable<object> calls = GetReceivedCalls();

                await Verify(new
                {
                    install,
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