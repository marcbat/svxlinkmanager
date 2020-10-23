using Microsoft.AspNetCore.Components;

using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using Spotnik.Gui.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spotnik.Gui.ViewModels
{
  public class HomeBase : ComponentBase, INotifyPropertyChanged
  {
    private int channel;
    private bool readlog;

    public event PropertyChangedEventHandler PropertyChanged;

    protected override void OnInitialized()
    {
      base.OnInitialized();

      LoadChannels();

      PropertyChanged += (s,e) => { 
        if (e.PropertyName == nameof(Channel)) {
          readlog = false;

          RunRestart();

          //ReadLog();
        } 
      };
    }
    
    [Inject]
    public IRepositories Repositories { get; set; }

    public int Channel
    {
      get => channel;
      set {
        channel = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Channel)));
      }
    }

    public List<Channel> Channels { get; private set; }

    private void LoadChannels() => Channels = Repositories.Channels.GetAll().ToList();

    private void RunRestart()
    {
      var channel = Repositories.Channels.Get(Channel);

      ExecuteCommand("/etc/spotnik/stopsvxlink.sh");

      ReplaceConfig(channel);

      ExecuteCommand("/etc/spotnik/runsvxlink.sh");
    }

    private static void ExecuteCommand(string script)
    {
      Process p = new Process();
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.FileName = script;
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      p.WaitForExit();
    }

    private static void ReplaceConfig(Channel channel)
    {
      File.Copy("/etc/spotnik/svxlink.conf", "/etc/spotnik/svxlink.current", true);

      string text = File.ReadAllText("/etc/spotnik/svxlink.current");
      text = text.Replace("HOST=HOST", $"HOST={channel.Host}");
      text = text.Replace("AUTH_KEY=AUTH_KEY", $"AUTH_KEY={channel.AuthKey}");
      text = text.Replace("PORT=PORT", $"PORT={channel.Port}");
      text = text.Replace("CALLSIGN=CALLSIGN", $"CALLSIGN={channel.CallSign}");
      File.WriteAllText("/etc/spotnik/svxlink.current", text);
    }

    //private void ReadLog()
    //{
    //  readlog = true;

    //  var wh = new AutoResetEvent(false);
    //  var fsw = new FileSystemWatcher(".");
    //  fsw.Filter = "file-to-read";
    //  fsw.EnableRaisingEvents = true;
    //  fsw.Changed += (s, e) => wh.Set();

    //  var fs = new FileStream("/tmp/svxlink.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    //  using (var sr = new StreamReader(fs))
    //  {
    //    var s = "";
    //    while (readlog)
    //    {
    //      s = sr.ReadLine();
    //      if (s != null)
    //        Console.WriteLine($"toto:{s}");
    //      else
    //        wh.WaitOne(1000);
    //    }
    //  }

    //  wh.Close();
    //}

  }

}
