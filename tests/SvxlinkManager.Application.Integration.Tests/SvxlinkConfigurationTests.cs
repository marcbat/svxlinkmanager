using FluentAssertions;

using LiteDB;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SvxlinkManager.Application.SvxlinkManagerConfigs;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Queries;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Echolinks.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Echolinks.Queries;
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

        [Test]
        public async Task UpdateSvxlinkChannelCommand_WhenIsValid_ShouldUpdateSvxlinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                var channelId = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", 80, "Fake report callSign", new byte[] { 0x01, 0x02, 0x03 }));

                await mediatr.Send(new UpdateSvxlinkChannelCommand(configGuid, channelId, "Fake channel update", "Fake Host update", "Fake callSign update", 8080, "Fake report callSign update", new byte[] { 0x01, 0x02, 0x03 }));

                var svxlinkChannel = await mediatr.Send(new GetSvxlinkChannelByIdQuery(configGuid, channelId));

                svxlinkChannel.Should().NotBeNull();
                svxlinkChannel.Name.Should().Be("Fake channel update");
                svxlinkChannel.Host.Should().Be("Fake Host update");
                svxlinkChannel.CallSign.Should().Be("Fake callSign update");
                svxlinkChannel.Port.Should().Be(8080);
                svxlinkChannel.ReportCallSign.Should().Be("Fake report callSign update");
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

        [Test]
        public async Task DeleteSvxlinkChannelCommand_WhenIsValid_ShouldDeleteSvxlinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                var channelId = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", 80, "Fake report callSign", new byte[] { 0x01, 0x02, 0x03 }));

                await mediatr.Send(new DeleteSvxlinkChannelCommand(configGuid, channelId));

                Func<Task> act = async() => await mediatr.Send(new GetSvxlinkChannelByIdQuery(configGuid, channelId));

                await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le svxlink channel.");
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
        public async Task AddEcholinkChannelCommand_WhenIsValid_ShouldAddNewEcholinkChannel()
        {
            try
            {
                // Arrange
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());
                var channelName = "Fake Name";
                var host = "Fake Host";
                var callSign = "Fake Callsign";
                var password = "Fake Password";
                var sysopName = "Fake SysopName";
                var location = "Fake Location";
                var maxQso = 3;
                var description = "Fake Description";
                var soundGuid = new byte[] { 0x01, 0x02, 0x03 };

                var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, channelName, host, callSign, password, sysopName, location, maxQso, description, soundGuid));

                // Act
                var echolinkChannel = await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

                // Assert
                echolinkChannel.Should().NotBeNull();
                echolinkChannel.Name.Should().Be(channelName);
                echolinkChannel.Host.Should().Be(host);
                echolinkChannel.CallSign.Should().Be(callSign);
                echolinkChannel.Password.Should().Be(password);
                echolinkChannel.SysopName.Should().Be(sysopName);
                echolinkChannel.Location.Should().Be(location);
                echolinkChannel.MaxQso.Should().Be(maxQso);
                echolinkChannel.Description.Should().Be(description);
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
        public async Task UpdateEcholinkChannelCommand_WhenIsValid_ShouldUpdateEcholinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, "Fake Name", "Fake Host", "Fake Callsign", "Fake Password", "Fake SysopName", "Fake Location", 3, "Fake Description", new byte[] { 0x01, 0x02, 0x03 }));

                await mediatr.Send(new UpdateEcholinkChannelCommand(configGuid, channelId, "Fake Name Update", "Fake Host Update", "Fake Callsign Update", "Fake Password Update", "Fake SysopName Update", "Fake Location Update", 5, "Fake Description Update", new byte[] { 0x01, 0x02, 0x03 }));

                var echolinkChannel = await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

                echolinkChannel.Should().NotBeNull();
                echolinkChannel.Name.Should().Be("Fake Name Update");
                echolinkChannel.Host.Should().Be("Fake Host Update");
                echolinkChannel.CallSign.Should().Be("Fake Callsign Update");
                echolinkChannel.Password.Should().Be("Fake Password Update");
                echolinkChannel.SysopName.Should().Be("Fake SysopName Update");
                echolinkChannel.Location.Should().Be("Fake Location Update");
                echolinkChannel.MaxQso.Should().Be(5);
                echolinkChannel.Description.Should().Be("Fake Description Update");
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
        public async Task DeleteEcholinkChannelCommand_WhenIsValid_ShouldDeleteEcholinkChannel()
        {
            try
            {
                var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand());

                var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, "Fake Name", "Fake Host", "Fake Callsign", "Fake Password", "Fake SysopName", "Fake Location", 3, "Fake Description", new byte[] { 0x01, 0x02, 0x03 }));

                await mediatr.Send(new DeleteEcholinkChannelCommand(configGuid, channelId));

                Func<Task> act = async () => await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

                await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le echolink channel.");
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