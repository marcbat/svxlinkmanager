using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Integration.Tests.SMC.RP.Queries
{
    internal class GetRadioProfilByIdQueryTests : BaseTest
    {
        [Test]
        public async Task GetRadioProfilByIdQuery_ShouldGetRadioProfilById()
        {
            try
            {
                // Arrange
                var radioProfilGuid = Guid.NewGuid();
                var install = from id in CreateDefaultConfig()
                              from conf in svxlinkManagerConfigRepository.GetConfig(id)
                              from radioProfil in RadioProfil.Create(radioProfilGuid, "RadioProfilToGet", "145.500", "145.800", "1", "67", "88.5", "1", "1", "1", "1", "1")
                              from _ in conf.AddRadioProfil(radioProfil)
                              from __ in svxlinkManagerConfigRepository.UpdateAsync(conf)
                              select conf;
                ClearReceivedCalls();
                var query = new GetRadioProfilByIdQuery(configGuid, radioProfilGuid);
                
                // Act
                var result = await mediatr.Send(query);
                
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
