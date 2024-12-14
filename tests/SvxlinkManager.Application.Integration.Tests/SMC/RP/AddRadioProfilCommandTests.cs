using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.RP
{
    internal class AddRadioProfilCommandTests : BaseTest
    {
        [Test]
        public async Task AddRadioProfilCommand_ShouldAddRadioProfil()
        {
            try
            {
                // Arrange
                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                var command = new AddRadioProfilCommand(configGuid, "AddedRadioProfil", "145.500", "145.800", "1", "67", "88.5", "1", "1", "1", "1", "1");

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
