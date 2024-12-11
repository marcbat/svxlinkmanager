using NSubstitute;

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

                var calls = authentificationService.ReceivedCalls()
                    .Concat(sa818Service.ReceivedCalls())
                    .Concat(svxlinkService.ReceivedCalls())
                    .Select(x => new { name = x.GetMethodInfo().Name, sequence = x.GetSequenceNumber(), arguments = x.GetArguments() })
                    .OrderBy(x => x.sequence)
                    .Select(x => new { x.name, x.arguments });

                await Verify(new
                {
                    install,
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