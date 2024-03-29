using FluentAssertions;

using LiteDB;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SvxlinkManager.Application.SvxlinkManagerConfigs;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Queries;
using SvxlinkManager.Domain.Aggregates;
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

            //var mapper = BsonMapper.Global;

            //mapper.Entity<SvxlinkManagerConfigAggregate>()
            //    .Field(x => x.SvxlinkChannels, "SvxlinkChannels");
        }

        [Test]
        public async Task AddSvxlinkChannelCommand_WhenIsValid_ShouldAddNewSvxlinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                var channelId = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", 80, "Fake report callSign", new byte[] { 0x01, 0x02, 0x03 }));

                var svxlinkChannel = await mediatr.Send(new GetSvxlinkChannelByIdQuery(configGuid, channelId));

                svxlinkChannel.Should().NotBeNull();
                svxlinkChannel.Name.Should().Be("Fake channel");
                svxlinkChannel.Host.Should().Be("Fake Host");
                svxlinkChannel.CallSign.Should().Be("Fake callSign");
                svxlinkChannel.Port.Should().Be(80);
                svxlinkChannel.ReportCallSign.Should().Be("Fake report callSign");
                svxlinkChannel.SoundGuid.Should().NotBeEmpty();

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