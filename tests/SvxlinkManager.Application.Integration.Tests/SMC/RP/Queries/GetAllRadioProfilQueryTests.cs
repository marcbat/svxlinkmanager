using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.RP.Queries
{
    internal class GetAllRadioProfilQueryTests :BaseTest
    {
        [Test]
        public async Task GetAllRadioProfilQuery_ShouldGetAllRadioProfil()
        {
            try
            {
                // Arrange
                var install = CreateDefaultConfig();
                ClearReceivedCalls();

                var query = new GetAllRadioProfilQuery(configGuid);
                
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
