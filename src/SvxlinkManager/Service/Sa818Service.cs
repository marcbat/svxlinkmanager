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
    private readonly ILogger<Sa818Service> logger;
    private readonly string device = "/dev/ttyS2";
    private readonly int volume = 2;
    private readonly int mode = 1;
    private readonly string filter = "0,0,0";

    public Sa818Service(ILogger<Sa818Service> logger)
    {
      this.logger = logger;
    }

    public void WriteRadioProfile(RadioProfile radioProfile)
    {
      logger.LogInformation($"Application du profil {radioProfile.Name}.");

      WriteModuleGroup(radioProfile);
    }

    private void WriteModuleGroup(RadioProfile radioProfile)
    {
      var serial = new SerialPort
      {
        PortName = device,
        BaudRate = 9600,
        Parity = Parity.None,
        StopBits = StopBits.One,
        DataBits = 8,

        WriteTimeout = 2000,
        ReadTimeout = 2000
      };

      serial.DataReceived += Serial_DataReceived;

      serial.Open();

      var message = $"AT+DMOSETGROUP={mode},{radioProfile.RxFequ}0,{radioProfile.TxFrequ}0,{radioProfile.RxCtcss},{radioProfile.Squelch},{radioProfile.TxCtcss}\r\n";

      serial.WriteLine(message);

      Thread.Sleep(1000);

      serial.Close();
    }

    private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      string data = ((SerialPort)sender).ReadLine();

      logger.LogInformation($"Eéponse de SA818: {data}.");
    }
  }
}
