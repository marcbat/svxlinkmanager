using NSubstitute;

namespace SvxlinkManager.Application.Integration.Tests
{
    public class InstallCommandTests : BaseTest
    {
        //[Test]
        //public async Task AddSvxlinkChannelCommand_WhenIsValid_ShouldAddNewSvxlinkChannel()
        //{
        //    try
        //    {

        //        // arrange 
        //        var configGuid = Guid.NewGuid();
        //        var createConfig = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(configGuid));
        //        var addChannel = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", "Fake Password", 80, "Fake report callSign", "SoundTest", new byte[] { 0x01, 0x02, 0x03 }));

        //        // act
        //        var result = from configId in createConfig
        //                     from channelId in addChannel
        //                     select (configId, channelId);

        //        // assert
        //        var config = svxlinkManagerConfigRepository.GetConfig(configGuid);
        //        await Verify(new
        //        {
        //            result,
        //            config
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task InstallChannelsCommand_WhenIsValid_ShouldInstallChannels()
        //{
        //    try
        //    {
        //        // arrange
        //        var configGuid = Guid.NewGuid();
        //        var createConfig = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(configGuid));

        //        var installChannels = new List<Guid> {
        //            Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"),
        //            Guid.Parse("1f2e87b8-d984-4c05-8a4a-ffad65c829a9"),
        //            Guid.Parse("0f669a03-dcf1-4277-9b07-54f6a0fd3037"),
        //            Guid.Parse("a749ffe5-16c7-45da-809d-c048908f115c"),
        //            Guid.Parse("dd03fd9e-aeed-457e-97bf-973837a5fcec"),
        //            Guid.Parse("d4c59d86-947c-4b1d-831a-807c1877d426"),
        //            Guid.Parse("9f99b18b-96ea-453d-b07a-7923c09c939f"),
        //            Guid.Parse("dcc5afa2-790f-40ca-b24d-bf91e90b1ac7")
        //        };

        //        var command = new InstallChannelsCommand(configGuid, installChannels, "Fake CallSign", "Fake AnnonceCallSign");
        //        var install = await mediatr.Send(command);

        //        // act
        //        var result = createConfig
        //            .Bind(x => install);

        //        // assert
        //        var config = svxlinkManagerConfigRepository.GetConfig(configGuid);
        //        await Verify(new
        //        {
        //            result,
        //             config
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task SetDefaultChannelCommand_WhenIsValid_ShoudSetDefaultChannel()
        //{
        //    try
        //    {
        //        // arrange
        //        var configGuid = Guid.NewGuid();
        //        var createConfig = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(configGuid));

        //        var installChannels = new List<Guid> {
        //            Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"),
        //            Guid.Parse("1f2e87b8-d984-4c05-8a4a-ffad65c829a9"),
        //            Guid.Parse("0f669a03-dcf1-4277-9b07-54f6a0fd3037"),
        //            Guid.Parse("a749ffe5-16c7-45da-809d-c048908f115c"),
        //            Guid.Parse("dd03fd9e-aeed-457e-97bf-973837a5fcec"),
        //            Guid.Parse("d4c59d86-947c-4b1d-831a-807c1877d426"),
        //            Guid.Parse("9f99b18b-96ea-453d-b07a-7923c09c939f"),
        //            Guid.Parse("dcc5afa2-790f-40ca-b24d-bf91e90b1ac7")
        //        };

        //        var installCommand = new InstallChannelsCommand(configGuid, installChannels, "Fake CallSign", "Fake AnnonceCallSign");
        //        var install = await mediatr.Send(installCommand);

        //        var setDefaultChannelCommand = new SetDefaultChannelCommand(configGuid, installChannels.First());
        //        var setDefault = await mediatr.Send(setDefaultChannelCommand);

        //        // act
        //        var result = createConfig
        //            .Bind(x => install)
        //            .Bind(x => setDefault);

        //        // assert
        //        var config = svxlinkManagerConfigRepository.GetConfig(configGuid);
        //        await Verify(new
        //        {
        //            result,
        //            config
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

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

        //[Test]
        //public async Task UpdateSvxlinkChannelCommand_WhenIsValid_ShouldUpdateSvxlinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", "Fake password", 80, "Fake report callSign", "SoundTest", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new UpdateSvxlinkChannelCommand(configGuid, channelId, "Fake channel update", "Fake Host update", "Fake callSign update", "Fake password", 8080, "Fake report callSign update", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        var svxlinkChannel = await mediatr.Send(new GetSvxlinkChannelByIdQuery(configGuid, channelId));

        //        svxlinkChannel.Should().NotBeNull();
        //        svxlinkChannel.Name.Should().Be("Fake channel update");
        //        svxlinkChannel.Host.Should().Be("Fake Host update");
        //        svxlinkChannel.CallSign.Should().Be("Fake callSign update");
        //        svxlinkChannel.Port.Should().Be(8080);
        //        svxlinkChannel.ReportCallSign.Should().Be("Fake report callSign update");
        //        svxlinkChannel.SoundGuid.Should().NotBeEmpty();

        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task DeleteSvxlinkChannelCommand_WhenIsValid_ShouldDeleteSvxlinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddSvxlinkChannelCommand(configGuid, "Fake channel", "Fake Host", "Fake callSign", "Fake Password", 80, "Fake report callSign", "SoundTest", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new DeleteSvxlinkChannelCommand(configGuid, channelId));

        //        Func<Task> act = async() => await mediatr.Send(new GetSvxlinkChannelByIdQuery(configGuid, channelId));

        //        await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le svxlink channel.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task AddEcholinkChannelCommand_WhenIsValid_ShouldAddNewEcholinkChannel()
        //{
        //    try
        //    {
        //        // Arrange
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));
        //        var channelName = "Fake Name";
        //        var host = "Fake Host";
        //        var callSign = "Fake Callsign";
        //        var password = "Fake Password";
        //        var sysopName = "Fake SysopName";
        //        var location = "Fake Location";
        //        var maxQso = 3;
        //        var description = "Fake Description"; 
        //        var soundName = "Test Sound";
        //        var soundFile = new byte[] { 0x01, 0x02, 0x03 };

        //        var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, channelName, host, callSign, password, sysopName, location, maxQso, description, soundName, soundFile));

        //        // Act
        //        var echolinkChannel = await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

        //        // Assert
        //        echolinkChannel.Should().NotBeNull();
        //        echolinkChannel.Name.Should().Be(channelName);
        //        echolinkChannel.Host.Should().Be(host);
        //        echolinkChannel.CallSign.Should().Be(callSign);
        //        echolinkChannel.Password.Should().Be(password);
        //        echolinkChannel.SysopName.Should().Be(sysopName);
        //        echolinkChannel.Location.Should().Be(location);
        //        echolinkChannel.MaxQso.Should().Be(maxQso);
        //        echolinkChannel.Description.Should().Be(description);
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task UpdateEcholinkChannelCommand_WhenIsValid_ShouldUpdateEcholinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, "Fake Name", "Fake Host", "Fake Callsign", "Fake Password", "Fake SysopName", "Fake Location", 3, "Fake Description", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new UpdateEcholinkChannelCommand(configGuid, channelId, "Fake Name Update", "Fake Host Update", "Fake Callsign Update", "Fake Password Update", "Fake SysopName Update", "Fake Location Update", 5, "Fake Description Update", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        var echolinkChannel = await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

        //        echolinkChannel.Should().NotBeNull();
        //        echolinkChannel.Name.Should().Be("Fake Name Update");
        //        echolinkChannel.Host.Should().Be("Fake Host Update");
        //        echolinkChannel.CallSign.Should().Be("Fake Callsign Update");
        //        echolinkChannel.Password.Should().Be("Fake Password Update");
        //        echolinkChannel.SysopName.Should().Be("Fake SysopName Update");
        //        echolinkChannel.Location.Should().Be("Fake Location Update");
        //        echolinkChannel.MaxQso.Should().Be(5);
        //        echolinkChannel.Description.Should().Be("Fake Description Update");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task DeleteEcholinkChannelCommand_WhenIsValid_ShouldDeleteEcholinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddEcholinkChannelCommand(configGuid, "Fake Name", "Fake Host", "Fake Callsign", "Fake Password", "Fake SysopName", "Fake Location", 3, "Fake Description", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new DeleteEcholinkChannelCommand(configGuid, channelId));

        //        Func<Task> act = async () => await mediatr.Send(new GetEchoLinkChannelByIdQuery(configGuid, channelId));

        //        await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le echolink channel.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task AddReflectorCommand_WhenIsValid_ShouldAddNewReflector()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        // Arrange
        //        var reflectorName = "Fake Name";
        //        var reflectorConfig = "Fake Config";

        //        var reflectorId = await mediatr.Send(new AddReflectorCommand(configGuid, reflectorName, reflectorConfig));

        //        var reflector = await mediatr.Send(new GetReflectorByIdQuery(configGuid, reflectorId));

        //        reflector.Should().NotBeNull();
        //        reflector.Name.Should().Be(reflectorName);
        //        reflector.Config.Should().Be(reflectorConfig);
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task UpdateReflectorCommand_WhenIsValid_ShouldUpdateReflector()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var reflectorId = await mediatr.Send(new AddReflectorCommand(configGuid, "Fake Name", "Fake Config"));

        //        await mediatr.Send(new UpdateReflectorCommand(configGuid, reflectorId, "Fake Name Update", "Fake Config Update"));

        //        var reflector = await mediatr.Send(new GetReflectorByIdQuery(configGuid, reflectorId));

        //        reflector.Should().NotBeNull();
        //        reflector.Name.Should().Be("Fake Name Update");
        //        reflector.Config.Should().Be("Fake Config Update");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task DeleteReflectorCommand_WhenIsValid_ShouldDeleteReflector()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var reflectorId = await mediatr.Send(new AddReflectorCommand(configGuid, "Fake Name", "Fake Config"));

        //        await mediatr.Send(new DeleteReflectorCommand(configGuid, reflectorId));

        //        Func<Task> act = async () => await mediatr.Send(new GetReflectorByIdQuery(configGuid, reflectorId));

        //        await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le réflecteur.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[TestCase("123.45", "543.21", "5", "67", "67", "10", "0.5", "100", "10000", "true")]
        //[TestCase("123.45", "543.21", "5", null, "67", "10", "0.5", "100", "10000", "true")]
        //[TestCase("123.45", "543.21", "5", "67", null, "10", "0.5", "100", "10000", "true")]
        //public async Task AddRadioProfilCommand_WhenIsValid_ShouldAddNewRadioProfil(string rxFrequency, string txFrequency, string squelch, string? txCtcss, string? rxCtCss, string volume, string preEmph, string highPass, string lowPass, string squelchDetection)
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        // Arrange
        //        var name = "Fake Name";

        //        var radioProfilId = await mediatr.Send(new AddRadioProfilCommand(configGuid, name,
        //                       rxFrequency,
        //                       txFrequency,
        //                       squelch,
        //                       txCtcss,
        //                       rxCtCss,
        //                       volume,
        //                       preEmph,
        //                       highPass,
        //                       lowPass,
        //                       squelchDetection));

        //        var radioProfil = await mediatr.Send(new GetRadioProfilByIdQuery(configGuid, radioProfilId));

        //        radioProfil.Should().NotBeNull();
        //        radioProfil.Name.Should().Be(name);
        //        radioProfil.RxFequency.Should().Be(rxFrequency);
        //        radioProfil.TxFrequency.Should().Be(txFrequency);
        //        radioProfil.Squelch.Should().Be(squelch);
        //        radioProfil.TxCtcss.Should().Be(txCtcss);
        //        radioProfil.RxCtCss.Should().Be(rxCtCss);
        //        radioProfil.Volume.Should().Be(volume);
        //        radioProfil.PreEmph.Should().Be(preEmph);
        //        radioProfil.HightPass.Should().Be(highPass);
        //        radioProfil.LowPass.Should().Be(lowPass);
        //        radioProfil.SquelchDetection.Should().Be(squelchDetection);


        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task UpdateRadioProfilCommand_WhenIsValid_ShouldUpdateRadioProfil()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var radioProfilId = await mediatr.Send(new AddRadioProfilCommand(configGuid, "Fake Name", "123.45", "543.21", "5", "67", "67", "10", "0.5", "100", "10000", "true"));

        //        await mediatr.Send(new UpdateRadioProfilCommand(configGuid, radioProfilId, "Fake Name Update", "111.11", "222.22", "6", "77", "77", "20", "1.0", "200", "20000", "false"));

        //        var radioProfil = await mediatr.Send(new GetRadioProfilByIdQuery(configGuid, radioProfilId));

        //        radioProfil.Should().NotBeNull();
        //        radioProfil.Name.Should().Be("Fake Name Update");
        //        radioProfil.RxFequency.Should().Be("111.11");
        //        radioProfil.TxFrequency.Should().Be("222.22");
        //        radioProfil.Squelch.Should().Be("6");
        //        radioProfil.TxCtcss.Should().Be("77");
        //        radioProfil.RxCtCss.Should().Be("77");
        //        radioProfil.Volume.Should().Be("20");
        //        radioProfil.PreEmph.Should().Be("1.0");
        //        radioProfil.HightPass.Should().Be("200");
        //        radioProfil.LowPass.Should().Be("20000");
        //        radioProfil.SquelchDetection.Should().Be("false");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task DeleteRadioProfilCommand_WhenIsValid_ShouldDeleteRadioProfil()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var radioProfilId = await mediatr.Send(new AddRadioProfilCommand(configGuid, "Fake Name", "123.45", "543.21", "5", "67", "67", "10", "0.5", "100", "10000", "true"));

        //        await mediatr.Send(new DeleteRadioProfilCommand(configGuid, radioProfilId));

        //        Func<Task> act = async () => await mediatr.Send(new GetRadioProfilByIdQuery(configGuid, radioProfilId));

        //        await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le profil radio.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task AddAvanceSvxlinkChannelCommand_WhenIsValid_ShouldAddNewAdvanceSvxlinkChannel()
        //{
        //    try
        //    {
        //        // Arrange
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddAdvanceSvxlinkChannelCommand(configGuid, "Name",
        //                                "SvxlinkConf",
        //                                "ModuleDtmfRepeater",
        //                                "ModuleEchoLink",
        //                                "ModuleFrn",
        //                                "ModuleHelp",
        //                                "ModuleMetarInfo",
        //                                "ModuleParrot",
        //                                "ModulePropagationMonitor",
        //                                "ModuleSelCallEnc",
        //                                "ModuleTclVoiceMail",
        //                                "ModuleTrx", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        // Act
        //        var advanceSvxlinkChannel = await mediatr.Send(new GetAvanceSvxlinkChannelByIdQuery(configGuid, channelId));

        //        // Assert
        //        advanceSvxlinkChannel.Should().NotBeNull();
        //        advanceSvxlinkChannel.Name.Should().Be("Name");
        //        advanceSvxlinkChannel.SvxlinkConf.Should().Be("SvxlinkConf");
        //        advanceSvxlinkChannel.ModuleDtmfRepeater.Should().Be("ModuleDtmfRepeater");
        //        advanceSvxlinkChannel.ModuleEchoLink.Should().Be("ModuleEchoLink");
        //        advanceSvxlinkChannel.ModuleFrn.Should().Be("ModuleFrn");
        //        advanceSvxlinkChannel.ModuleHelp.Should().Be("ModuleHelp");
        //        advanceSvxlinkChannel.ModuleMetarInfo.Should().Be("ModuleMetarInfo");
        //        advanceSvxlinkChannel.ModuleParrot.Should().Be("ModuleParrot");
        //        advanceSvxlinkChannel.ModulePropagationMonitor.Should().Be("ModulePropagationMonitor");
        //        advanceSvxlinkChannel.ModuleSelCallEnc.Should().Be("ModuleSelCallEnc");
        //        advanceSvxlinkChannel.ModuleTclVoiceMail.Should().Be("ModuleTclVoiceMail");
        //        advanceSvxlinkChannel.ModuleTrx.Should().Be("ModuleTrx");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task UpdateAdvanceSvxlinkChannelCommand_WhenIsValid_ShouldUpdateAdvanceSvxlinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddAdvanceSvxlinkChannelCommand(configGuid, "Fake Name", "Fake SvxlinkConf", "Fake ModuleDtmfRepeater", "Fake ModuleEchoLink", "Fake ModuleFrn", "Fake ModuleHelp", "Fake ModuleMetarInfo", "Fake ModuleParrot", "Fake ModulePropagationMonitor", "Fake ModuleSelCallEnc", "Fake ModuleTclVoiceMail", "Fake ModuleTrx", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new UpdateAdvanceSvxlinkChannelCommand(configGuid, channelId, "Fake Name Update", "Fake SvxlinkConf Update", "Fake ModuleDtmfRepeater Update", "Fake ModuleEchoLink Update", "Fake ModuleFrn Update", "Fake ModuleHelp Update", "Fake ModuleMetarInfo Update", "Fake ModuleParrot Update", "Fake ModulePropagationMonitor Update", "Fake ModuleSelCallEnc Update", "Fake ModuleTclVoiceMail Update", "Fake ModuleTrx Update", "Sound Test", new byte[] { 0x04, 0x05, 0x06 }));

        //        var avanceSvxlinkChannel = await mediatr.Send(new GetAvanceSvxlinkChannelByIdQuery(configGuid, channelId));

        //        avanceSvxlinkChannel.Should().NotBeNull();
        //        avanceSvxlinkChannel.Name.Should().Be("Fake Name Update");
        //        avanceSvxlinkChannel.SvxlinkConf.Should().Be("Fake SvxlinkConf Update");
        //        avanceSvxlinkChannel.ModuleDtmfRepeater.Should().Be("Fake ModuleDtmfRepeater Update");
        //        avanceSvxlinkChannel.ModuleEchoLink.Should().Be("Fake ModuleEchoLink Update");
        //        avanceSvxlinkChannel.ModuleFrn.Should().Be("Fake ModuleFrn Update");
        //        avanceSvxlinkChannel.ModuleHelp.Should().Be("Fake ModuleHelp Update");
        //        avanceSvxlinkChannel.ModuleMetarInfo.Should().Be("Fake ModuleMetarInfo Update");
        //        avanceSvxlinkChannel.ModuleParrot.Should().Be("Fake ModuleParrot Update");
        //        avanceSvxlinkChannel.ModulePropagationMonitor.Should().Be("Fake ModulePropagationMonitor Update");
        //        avanceSvxlinkChannel.ModuleSelCallEnc.Should().Be("Fake ModuleSelCallEnc Update");
        //        avanceSvxlinkChannel.ModuleTclVoiceMail.Should().Be("Fake ModuleTclVoiceMail Update");
        //        avanceSvxlinkChannel.ModuleTrx.Should().Be("Fake ModuleTrx Update");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}

        //[Test]
        //public async Task DeleteAdvanceSvxlinkChannelCommand_WhenIsValid_ShouldDeleteAdvanceSvxlinkChannel()
        //{
        //    try
        //    {
        //        var configGuid = await mediatr.Send(new CreateSvxlinkManagerConfigCommand(Guid.NewGuid()));

        //        var channelId = await mediatr.Send(new AddAdvanceSvxlinkChannelCommand(configGuid, "Fake Name", "Fake SvxlinkConf", "Fake ModuleDtmfRepeater", "Fake ModuleEchoLink", "Fake ModuleFrn", "Fake ModuleHelp", "Fake ModuleMetarInfo", "Fake ModuleParrot", "Fake ModulePropagationMonitor", "Fake ModuleSelCallEnc", "Fake ModuleTclVoiceMail", "Fake ModuleTrx", "Sound Test", new byte[] { 0x01, 0x02, 0x03 }));

        //        await mediatr.Send(new DeleteAdvanceSvxlinkChannelCommand(configGuid, channelId));

        //        Func<Task> act = async () => await mediatr.Send(new GetAvanceSvxlinkChannelByIdQuery(configGuid, channelId));

        //        await act.Should().ThrowAsync<SvxlinkManagerException>().WithMessage("Impossible de récupérer le canal avancé Svxlink.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    finally
        //    {
        //        File.Delete(db);
        //    }
        //}


    }

    
}