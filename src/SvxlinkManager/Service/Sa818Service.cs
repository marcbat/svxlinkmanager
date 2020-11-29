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
  public class Sa818Service
  {
    #region Fields

    private readonly string device = "/dev/ttyS2";
    private readonly ILogger<Sa818Service> logger;
    private readonly int mode = 1;

    #endregion Fields

    #region Constructors

    public Sa818Service(ILogger<Sa818Service> logger)
    {
      this.logger = logger;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Create serial port configuration
    /// </summary>
    /// <returns>The SerialPort</returns>
    public SerialPort GetSerialPort()
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
    /// Writes the radio profile into the serial port of SA818 module
    /// </summary>
    /// <param name="radioProfile">Selected radio profile</param>
    public void WriteRadioProfile(RadioProfile radioProfile)
    {
      logger.LogInformation($"Application du profil {radioProfile.Name}.");

      WriteModule($"AT+DMOSETGROUP={mode},{radioProfile.RxFequ}0,{radioProfile.TxFrequ}0,{radioProfile.RxCtcss},{radioProfile.Squelch},{radioProfile.TxCtcss}\r\n");
      WriteModule($"AT+DMOSETVOLUME={radioProfile.Volume}\r\n");
      WriteModule($"AT+SETFILTER={radioProfile.PreEmph},{radioProfile.HightPass},{radioProfile.LowPass}\r\n");
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

      logger.LogInformation($"Eéponse de SA818: {data}.");
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

        logger.LogInformation($"Envoi du message : {message}");

        serial.WriteLine(message);

        Thread.Sleep(1000);

        serial.Close();
      }
    }

    #endregion Methods
  }
}