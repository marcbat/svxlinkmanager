using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.Extensions;

using NUnit.Framework;

using SvxlinkManager.Models;
using SvxlinkManager.Repositories;
using SvxlinkManager.Service;

using SvxlinkManagerTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service.Tests
{
  [TestFixture()]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Non-substitutable member", "NS1001:Non-virtual setup specification.", Justification = "Marche quand même.")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Non-substitutable member", "NS1004:Argument matcher used with a non-virtual member of a class.", Justification = "Marche quand même.")]
  public class SvxLinkServiceTests
  {
    private ILogger<SvxLinkService> logger;
    private IRepositories repositories;

    [SetUp]
    public void Setup()
    {
      logger = Substitute.For<ILogger<SvxLinkService>>();
      repositories = Substitute.For<IRepositories>();
    }

    [Test(Description = "Test le log Connected nodes")]
    public void ParseLogConnectedTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      Channel returnedChannel = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Connected += c => returnedChannel = c;

      // act
      service.ParseLog(channel, "ReflectorLogic: Connected nodes: EXPERIMENTAL, (25) F1POK H, (25) F1POK V, ( M) F1POK H, (05) F5HII H, (27) F6DSB V, (CH) HB9GVE H, (CH) HB9GXP H");

      // assert
      Assert.AreEqual(channel, returnedChannel);
      Assert.AreEqual(8, service.Nodes.Count);
    }

    [Test(Description = "Test le log SIGTERM")]
    public void ParseLogDisconnectedTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      bool disconnected = false;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Disconnected += () => disconnected = true;

      // act
      service.ParseLog(channel, "SIGTERM");

      // assert
      Assert.IsTrue(disconnected);
      Assert.IsEmpty(service.Nodes);
    }

    [Test(Description = "Test le log Node left")]
    public void ParseLogNodeDisconnectedTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      Node node = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.NodeDisconnected += (n) => node = n;
      service.Nodes.Add(new Node { Name = " (CH) HB9GXP H" });

      // act
      service.ParseLog(channel, "ReflectorLogic: Node left: (CH) HB9GXP H");

      // assert
      Assert.AreEqual(node.Name, " (CH) HB9GXP H");
      Assert.IsEmpty(service.Nodes);
    }

    [Test(Description = "Test le log Node joined")]
    public void ParseLogNodeConnectedTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      Node node = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.NodeConnected += (n) => node = n;

      // act
      service.ParseLog(channel, "ReflectorLogic: Node joined: (CH) HB9GXP H");

      // assert
      Assert.AreEqual(node.Name, " (CH) HB9GXP H");
      Assert.AreEqual(1, service.Nodes.Count);
    }

    [Test(Description = "Test le log Talker start")]
    public void ParseLogNodeTxTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      Node node = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Nodes.Add(new Node { Name = " (CH) HB9GXP H" });
      service.NodeTx += (n) => node = n;

      // act
      service.ParseLog(channel, "ReflectorLogic: Talker start: (CH) HB9GXP H");

      // assert
      Assert.AreEqual(node.Name, " (CH) HB9GXP H");
      Assert.AreEqual(node.ClassName, "node node-tx");
    }

    [Test(Description = "Test le log Talker stop")]
    public void ParseLogNodeRxTest()
    {
      // arrange
      var channel = new SvxlinkChannel();
      Node node = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Nodes.Add(new Node { Name = " (CH) HB9GXP H" });
      service.NodeRx += (n) => node = n;

      // act
      service.ParseLog(channel, "ReflectorLogic: Talker stop: (CH) HB9GXP H");

      // assert
      Assert.AreEqual(node.Name, " (CH) HB9GXP H");
      Assert.AreEqual(node.ClassName, "node");
    }

    [Test(Description = "Test le log Access denied")]
    public void ParseLogAccessDeniedTest()
    {
      // arrange
      var channel = new SvxlinkChannel { Name = "Default" };
      string message = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Nodes.Add(new Node { Name = " (CH) HB9GXP H" });
      service.Error += (t, m) => message = m;

      // act
      service.ParseLog(channel, "Access denied");

      // assert
      Assert.AreEqual(message, $"Impossible de se connecter au salon {channel.Name}. <br/> Accès refusé.");
    }

    [Test(Description = "Test le log Host not found")]
    public void ParseLogHostNotFoundTest()
    {
      // arrange
      var channel = new SvxlinkChannel { Name = "Default", Host = "Default Host" };
      string message = null;

      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.Nodes.Add(new Node { Name = " (CH) HB9GXP H" });
      service.Error += (t, m) => message = m;

      // act
      service.ParseLog(channel, "Host not found");

      // assert
      Assert.AreEqual(message, $"Impossible de se connecter au salon {channel.Name}. <br/> Server {channel.Host} introuvable.");
    }

    [Test(Description = "Test le changement de salon parrot")]
    public void SetChannelIdParrot()
    {
      // arrange
      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.When(x => x.Parrot()).DoNotCallBase();

      // act
      service.ChannelId = 1000;

      // assert
      service.Received(1).Parrot();
    }

    [Test(Description = "Test la deconnexion.")]
    public void SetChannelIdStop()
    {
      // arrange
      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.When(x => x.StopSvxlink()).DoNotCallBase();

      // act
      service.ChannelId = 1000;

      // assert
      service.Received(1).StopSvxlink();
    }

    [Test(Description = "Test l'activation d'un salon.")]
    public void SetChannelIdActivate()
    {
      // arrange
      var service = Substitute.ForPartsOf<SvxLinkService>(logger, repositories);
      service.When(x => x.ActivateChannel(30)).DoNotCallBase();

      // act
      service.ChannelId = 30;

      // assert
      service.Received(1).ActivateChannel(30);
    }

    [Test(Description = "Test le fonctinnement complet de l'activation du module Parrot")]
    public void ParrotTest()
    {
      // arrange
      var service = Substitute.ForPartsOf<TestableSvxlinkService>(logger, repositories);
      service.When(x => x.StopSvxlink()).DoNotCallBase();
      service.When(x => x.StartSvxLink()).DoNotCallBase();
      Predicate<Dictionary<string, Dictionary<string, string>>> isParametersOk = x => x["GLOBAL"]["LOGICS"] == "SimplexLogic" && x["SimplexLogic"]["MODULES"] == "ModuleParrot";

      // act
      service.Parrot();

      // assert
      service.Received(1).StopSvxlink();
      service.Received(1).Protected("CreateNewCurrentConfig");
      service.Received(1).Protected("ReplaceConfig", Arg.Any<string>(), Arg.Is<Dictionary<string, Dictionary<string, string>>>(x => isParametersOk(x)));
      service.Received(1).Protected("ReplaceSoundFile", Arg.Is<Channel>(x => x == null));
      service.Received(1).StartSvxLink();
      service.Received(1).Protected("ExecuteCommand", "echo '1#'> /tmp/dtmf_uhf");
    }

    [Test(Description = "Test la déconnection lors de l'utilisation du call par défaut")]
    public void ActivateSvxlinkChannelDefaultCallTest()
    {
      // arrange
      var service = Substitute.ForPartsOf<TestableSvxlinkService>(logger, repositories);
      service.When(x => x.StopSvxlink()).DoNotCallBase();
      var channel = new SvxlinkChannel { CallSign = "(CH) SVX4LINK H" };

      // act
      service.ActivateSvxlinkChannel(channel);

      // assert
      service.Received(1).StopSvxlink();
      service.DidNotReceive().Protected("CreateNewCurrentConfig");
    }

    [Test(Description = "Test de l'activation d'un channel Svxlink")]
    public void ActivateSvxlinkChannelTest()
    {
      // arrange
      var radioProfile = new RadioProfile { RxCtcss = "0002" };
      repositories.RadioProfiles.GetCurrent().Returns(radioProfile);

      var service = Substitute.ForPartsOf<TestableSvxlinkService>(logger, repositories);
      service.When(x => x.StopSvxlink()).DoNotCallBase();
      service.When(x => x.StartSvxLink(Arg.Any<SvxlinkChannel>())).DoNotCallBase();
      var channel = new SvxlinkChannel { CallSign = "(CH) HB9GXP H" };

      Predicate<Dictionary<string, Dictionary<string, string>>> isParametersOk = x =>
      {
        return x["GLOBAL"]["LOGICS"] == "SimplexLogic,ReflectorLogic"
            && x["SimplexLogic"]["MODULES"] == "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"
            && x["SimplexLogic"]["CALLSIGN"] == channel.ReportCallSign
            && x["SimplexLogic"]["REPORT_CTCSS"] == radioProfile.RxTone
            && x["Rx1"]["SQL_DET"] == radioProfile.SquelchDetection
            && x["ReflectorLogic"]["CALLSIGN"] == channel.CallSign
            && x["ReflectorLogic"]["HOST"] == channel.Host
            && x["ReflectorLogic"]["AUTH_KEY"] == channel.AuthKey
            && x["ReflectorLogic"]["PORT"] == channel.Port.ToString();
      };

      // act
      service.ActivateSvxlinkChannel(channel);

      // assert
      service.Received(1).StopSvxlink();
      service.Received(1).Protected("CreateNewCurrentConfig");
      service.Received(1).Protected("ReplaceConfig", $"{service.applicationPath}/SvxlinkConfig/svxlink.current", Arg.Is<Dictionary<string, Dictionary<string, string>>>(x => isParametersOk(x)));
      service.Received(1).Protected("ReplaceSoundFile", channel);
      service.Received(1).StartSvxLink(channel);
    }

    [Test(Description = "Test de l'activation d'un channel Echolink")]
    public void ActivateEcholinkTest()
    {
      // arrange
      var radioProfile = new RadioProfile { RxCtcss = "0002" };
      repositories.RadioProfiles.GetCurrent().Returns(radioProfile);

      var service = Substitute.ForPartsOf<TestableSvxlinkService>(logger, repositories);
      service.When(x => x.StopSvxlink()).DoNotCallBase();
      service.When(x => x.StartSvxLink(Arg.Any<EcholinkChannel>())).DoNotCallBase();
      var channel = new EcholinkChannel { CallSign = "HB9GXP-L" };

      Predicate<Dictionary<string, Dictionary<string, string>>> isParametersOk = x =>
      {
        return x["GLOBAL"]["LOGICS"] == "SimplexLogic"
            && x["SimplexLogic"]["MODULES"] == "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor,ModuleEchoLink,ModuleParrot"
            && x["SimplexLogic"]["CALLSIGN"] == channel.CallSign
            && x["SimplexLogic"]["REPORT_CTCSS"] == radioProfile.RxTone;
      };
      Predicate<Dictionary<string, Dictionary<string, string>>> isEcholinkParametersOk = x =>
      {
        return x["ModuleEchoLink"]["SERVERS"] == channel.Host
              && x["ModuleEchoLink"]["CALLSIGN"] == channel.CallSign
              && x["ModuleEchoLink"]["PASSWORD"] == channel.Password
              && x["ModuleEchoLink"]["SYSOPNAME"] == channel.SysopName
              && x["ModuleEchoLink"]["LOCATION"] == channel.Location
              && x["ModuleEchoLink"]["MAX_QSOS"] == channel.MaxQso.ToString()
              && x["ModuleEchoLink"]["MAX_CONNECTIONS"] == (channel.MaxQso + 1).ToString()
              && x["ModuleEchoLink"]["DESCRIPTION"] == channel.Description;
      };

      // act
      service.ActivateEcholink(channel);

      // assert
      service.Received(1).StopSvxlink();
      service.Received(1).Protected("CreateNewCurrentConfig");
      service.Received(1).Protected("ReplaceConfig", $"{service.applicationPath}/SvxlinkConfig/svxlink.current", Arg.Is<Dictionary<string, Dictionary<string, string>>>(x => isParametersOk(x)));
      service.Received(1).Protected("ReplaceConfig", $"{service.applicationPath}/SvxlinkConfig/svxlink.d/ModuleEchoLink.conf", Arg.Is<Dictionary<string, Dictionary<string, string>>>(x => isEcholinkParametersOk(x)));
      service.Received(1).Protected("ReplaceSoundFile", channel);
      service.Received(1).StartSvxLink(channel);
    }

    [Test(Description = "Test l'activation du canal par defaut.")]
    public void StartDefaultChannelTest()
    {
      // arrange
      var channel = new SvxlinkChannel { Id = 300 };
      repositories.Channels.GetDefault().Returns(channel);

      var service = Substitute.ForPartsOf<TestableSvxlinkService>(logger, repositories);
      service.When(x => x.ActivateChannel(Arg.Any<int>())).DoNotCallBase();

      // act
      service.StartDefaultChannel();

      // assert
      Assert.AreEqual(service.ChannelId, channel.Id);
      service.Received(1).ActivateChannel(channel.Id);
    }

    /// <summary>
    /// Mockup for SvxLinkService
    /// </summary>
    /// <seealso cref="SvxlinkManager.Service.SvxLinkService" />
    public class TestableSvxlinkService : SvxLinkService
    {
      public TestableSvxlinkService(ILogger<SvxLinkService> logger, IRepositories repositories) : base(logger, repositories)
      {
      }

      protected override void ReplaceConfig(string filePath, Dictionary<string, Dictionary<string, string>> parameters)
      {
      }

      protected override string ExecuteCommand(string cmd)
      {
        return string.Empty;
      }

      protected override void CreateNewCurrentConfig()
      {
      }

      protected override void SetDtmfWatcher()
      {
      }

      protected override void ReplaceSoundFile(Channel channel = null)
      {
      }
    }
  }
}