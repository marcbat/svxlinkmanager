using Fclp;
using Fclp.Internals;

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace _818cli
{
  class Options
  {
    public string Device { get; set; } = "/dev/ttyS2";

    public int Mode { get; set; } = 1;

    public string Group { get; set; }

    public int Volume { get; set; } = 2;

    public string Filter { get; set; } = "0,0,0";

  }

  public class Program
  {
    private static SerialPort serial;

    static void Main(string[] args)
    {
      Console.WriteLine("Entrée dans la methode");

      try
      {

        var p = new FluentCommandLineParser<Options>();

        p.Setup(arg => arg.Device)
          .As('d', "device");

        p.Setup(arg => arg.Group)
          .As('g', "group")
          .Required();

        p.Setup(arg => arg.Volume)
          .As('v', "volume");

        p.Setup(arg => arg.Filter)
         .As('f', "filter");

        Console.WriteLine("Parsing");
        var result = p.Parse(args);

        if (!result.HasErrors)
        {
          Console.WriteLine("Aucune erreur");
          serial = new SerialPort
          {
            PortName = p.Object.Device,
            BaudRate = 9600,
            Parity = Parity.None,
            StopBits = StopBits.One,
            DataBits = 8,

            WriteTimeout = 2000,
            ReadTimeout = 2000
          };

          serial.DataReceived += Serial_DataReceived;

          WriteModule(serial, p.Object);


        }
        else
        {
          Console.WriteLine($"Erreur : {result.ErrorText}");
        }

      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
      finally
      {
        serial.Dispose();
      }

    }

    private static void WriteModule(SerialPort serial, Options options)
    {
      Console.WriteLine("Ouverture du port.");

      serial.Open();

      var message = $"AT+DMOSETGROUP={options.Mode},{options.Group}\r\n";

      Console.WriteLine($"Ecriture de {message}");
      serial.WriteLine(message);

      Thread.Sleep(1000);

      Console.Write("Fermeture du port.");
      serial.Close();

    }

    private static void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      string data = serial.ReadLine();

      Console.WriteLine($"Valeur reçue: {data}");
    }
  }
}
