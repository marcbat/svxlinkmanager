using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.Extensions;

using NUnit.Framework;

using SvxlinkManager.Models;
using SvxlinkManager.Repositories;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service.Tests
{
  [TestFixture()]
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
  }
}