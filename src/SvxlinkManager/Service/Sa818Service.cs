using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public interface ISa818Service
  {
    void WriteRadioProfile(RadioProfile radioProfile);
  }

  public class Sa818Service : ISa818Service
  {
    private readonly string device = "/dev/ttyS2";
    private readonly ILogger<Sa818Service> logger;
    private readonly TelemetryClient telemetry;
    private readonly int mode = 1;

    public Sa818Service(ILogger<Sa818Service> logger, TelemetryClient telemetry)
    {
      this.logger = logger;
      this.telemetry = telemetry;
    }

    /// <summary>
    /// Writes the radio profile into the serial port of SA818 module
    /// </summary>
    /// <param name="radioProfile">Selected radio profile</param>
    public void WriteRadioProfile(RadioProfile radioProfile)
    {
      try
      {
        telemetry.TrackEvent("Write radio profile", radioProfile.TrackProperties);

        logger.LogInformation($"Application du profil {radioProfile.Name}.");

        WriteModule($"AT+DMOSETGROUP={mode},{radioProfile.TxFrequ}0,{radioProfile.RxFequ}0,{radioProfile.TxCtcss},{radioProfile.Squelch},{radioProfile.RxCtcss}\r\n");
        WriteModule($"AT+DMOSETVOLUME={radioProfile.Volume}\r\n");
        WriteModule($"AT+SETFILTER={radioProfile.PreEmph},{radioProfile.HightPass},{radioProfile.LowPass}\r\n");
      }
      catch (Exception e)
      {
        telemetry.TrackException(e, radioProfile.TrackProperties);
      }
    }

    /// <summary>
    /// Create serial port configuration
    /// </summary>
    /// <returns>The SerialPort</returns>
    private SerialPort GetSerialPort()
    {
      return new SerialPort
      {
        PortName = device,
        BaudRate = 9600,
        Parity = Parity.None,
        StopBits = StopBits.One,
        DataBits = 8,

        WriteTimeout = 2000,
        ReadTimeout = 2000
      };
    }

    /// <summary>
    /// Handles the DataReceived event of the Serial port.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">
    /// The <see cref="SerialDataReceivedEventArgs"/> instance containing the event data.
    /// </param>
    private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      string data = ((SerialPort)sender).ReadLine();

      telemetry.TrackTrace("Réponse de SA818", SeverityLevel.Information, new Dictionary<string, string> { { "response", data } });
      logger.LogInformation($"Réponse de SA818: {data}.");
    }

    /// <summary>
    /// Write a message to module serial port
    /// </summary>
    /// <param name="message">The message.</param>
    private void WriteModule(string message)
    {
      using (var serial = GetSerialPort())
      {
        serial.DataReceived += Serial_DataReceived;

        serial.Open();

        telemetry.TrackTrace("Envoi du message au SA818", SeverityLevel.Information, new Dictionary<string, string> { { "message", message } });

        logger.LogInformation($"Envoi du message : {message}");

        serial.WriteLine(message);

        Thread.Sleep(1000);

        serial.Close();
      }
    }
  }
}