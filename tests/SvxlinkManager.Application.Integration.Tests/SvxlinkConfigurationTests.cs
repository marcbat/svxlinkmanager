using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SvxlinkManager.Application.SvxlinkManagerConfig;
using SvxlinkManager.Application.SvxlinkManagerConfig.Channels.Commands;
using SvxlinkManager.Domain.Entities;
using SvxlinkManager.Infrastructure;

namespace SvxlinkManager.Application.Integration.Tests
{
    public class SvxlinkConfigurationTests
    {
        private IMediator mediatr;
        private string db;

        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(cfg => cfg.AddConsole());

            services.AddApplication();
            services.AddInfrastructure();

            if(!Directory.Exists("db"))
                Directory.CreateDirectory("db");

            db = Path.Combine("db", $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.db");

            services.AddSingleton<IOptions<SvxlinkManagerOptions>>(x => new FakeOptions(db));

            var serviceProvider = services.BuildServiceProvider();

            mediatr = serviceProvider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task AddSvxlinkChannelCommand_WhenIsValid_ShouldAddNewSvxlinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", 80, "Fake report callSign", new byte[] { 0x01, 0x02, 0x03 }));
            
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
        public async Task AddSvxlinkChannelCommand_WhenIsValid_ShouldAddNewSvxlinkChannel2()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", 80, "Fake report callSign", new byte[] { 0x01, 0x02, 0x03 }));

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

    public class FakeOptions(string file) : IOptions<SvxlinkManagerOptions>
    {
        private readonly string file = file;

        public SvxlinkManagerOptions Value => new(file);
    }
}