using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

using NSubstitute;

using NUnit.Framework;

using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service.Tests
{
  [TestFixture()]
  public class WifiServiceTests
  {
    [Test()]
    public void ParseDeviceConsoleOutputTest()
    {
      // assert
      var output = "IN-USE  SSID                         MODE   CHAN  RATE        SIGNAL  BARS  SECURITY\r\n" +
        "*       Maison                       Infra  1     130 Mbit/s  57      ***   WPA2\r\n" +
        "        Maison_Guest                 Infra  1     130 Mbit/s  57      ***   WPA2\r\n" +
        "        DIRECT-14-HP Officejet 5740  Infra  11    65 Mbit/s   30      *     WPA2\r\n" +
        "        Sunrise_2.4GHz_8F92E0        Infra  11    195 Mbit/s  14      *     WPA1 WPA2\r\n";

      var logger = Substitute.For<ILogger<WifiService>>();
      var telemetry = new TelemetryClient();

      // act
      var service = new WifiService(logger, telemetry);
      var devices = service.ParseDeviceConsoleOutput(output);

      // assert
      Assert.AreEqual(devices.Count, 4);
    }

    [Test()]
    public void ParseConnectionConsoleOutputTest()
    {
      // assert
      var output = "NAME                 UUID                                  TYPE      DEVICE\r\n" +
        "Maison 2             33de31e2-fffc-4e55-82cc-5c9db0104f8e  wifi      wlan0\r\n" +
        "BOX                  3b6269bd-ef19-43ff-b753-25d5ac06337e  wifi      --\r\n" +
        "Connexion filaire 1  23dca64d-1f09-378e-a284-257a8321afd7  ethernet  --\r\n" +
        "Honor 9              986cb8c6-fe8e-4951-903f-105aef31a3ab  wifi      --\r\n";

      var logger = Substitute.For<ILogger<WifiService>>();
      var telemetry = new TelemetryClient();

      // act
      var service = new WifiService(logger, telemetry);
      var connections = service.ParseConnectionConsoleOutput(output);

      // assert
      Assert.AreEqual(connections.Count, 4);
    }
  }
}