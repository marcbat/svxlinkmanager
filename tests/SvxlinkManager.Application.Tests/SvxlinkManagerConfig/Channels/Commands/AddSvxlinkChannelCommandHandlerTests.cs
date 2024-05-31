using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using FluentAssertions.Extensions;

using NUnit.Framework;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfig.Channels.Commands;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Commands;

namespace SvxlinkManager.Application.SvxlinkManagerConfig.Channels.Commands.Tests
{
    [TestFixture]
    public class AddSvxlinkChannelCommandHandlerTests
    {
        private ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private ISoundRepository _soundRepository;
        private ILogger<AddSvxlinkChannelCommandHandler> _logger;
        private AddSvxlinkChannelCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _svxlinkManagerConfigRepository = Substitute.For<ISvxlinkManagerConfigRepository>();
            _soundRepository = Substitute.For<ISoundRepository>();
            _logger = Substitute.For<ILogger<AddSvxlinkChannelCommandHandler>>();
            _handler = new AddSvxlinkChannelCommandHandler(_svxlinkManagerConfigRepository, _soundRepository, _logger);
        }

        [Test]
        public async Task Handle_ValidCommand_ShouldAddSvxlinkChannel()
        {
            // Arrange
            var configId = Guid.NewGuid();
            var name = "Test Channel";
            var host = "localhost";
            var callSign = "ABCD";
            var password = "fake password";
            var port = 1234;
            var reportCallSign = "EFGH";
            var soundName = "Test Sound";
            var soundFile = new byte[] { 0x01, 0x02, 0x03 };

            var command = new AddSvxlinkChannelCommand(configId, name, host, callSign, password, port, reportCallSign, soundName, soundFile);

            var config = SvxlinkManagerConfigAggregate.Create(Guid.NewGuid());
            _svxlinkManagerConfigRepository.GetConfigAsync(configId).Returns(config);

            var sound = new Sound(string.Empty, name, soundFile);
            _soundRepository.CreateAsyc(Arg.Any<Sound>()).Returns(Task.FromResult(sound));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            config.SvxlinkChannels.Should().ContainSingle();
            var svxlinkChannel = config.SvxlinkChannels.First();
            svxlinkChannel.Name.Should().Be(name);
            svxlinkChannel.Host.Should().Be(host);
            svxlinkChannel.CallSign.Should().Be(callSign);
            svxlinkChannel.Port.Should().Be(port);
            svxlinkChannel.ReportCallSign.Should().Be(reportCallSign);

            await _svxlinkManagerConfigRepository.Received(1).UpdateAsync(config);
            await _soundRepository.Received(1).CreateAsyc(Arg.Any<Sound>());

            _logger.Received(1).LogInformation("Un nouveau svxlink channel a été ajouté avec succès.");
        }

        [Test]
        public void Handle_ExceptionThrown_ShouldThrowSvxlinkManagerException()
        {
            // Arrange
            var configId = Guid.NewGuid();
            var name = "Test Channel";
            var host = "localhost";
            var callSign = "ABCD";
            var password = "fake password";
            var port = 1234;
            var reportCallSign = "EFGH";
            var soundName = "Test Sound";
            var soundFile = new byte[] { 0x01, 0x02, 0x03 };

            var command = new AddSvxlinkChannelCommand(configId, name, host, callSign, password, port, reportCallSign, soundName, soundFile);

            _svxlinkManagerConfigRepository.GetConfigAsync(configId).Throws(new Exception("Test Exception"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible d'ajouter un nouveau svxlink channel.");
            _logger.Received(1).LogError(Arg.Any<Exception>(), "Impossible d'ajouter un nouveau svxlink channel.");
        }
    }
}