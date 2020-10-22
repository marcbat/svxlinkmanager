using Microsoft.AspNetCore.Components;

using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using Spotnik.Gui.Services;
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
    private string channel;
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

          ReadLog();
        } 
      };
    }
    [Inject]
    public IConfigService ConfigService { get; set; }

    [Inject]
    public IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

    [Inject]
    public DashBoardService DashBoardService { get; set; }

    public string Channel
    {
      get => channel;
      set {
        channel = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Channel)));
      }
    }

    public List<Channel> Channels { get; private set; }

    private void LoadChannels()
    {
      using (var dbcontext = DbFactory.CreateDbContext())
        Channels = dbcontext.Channels.ToList();
    }

    private void RunRestart()
    {
      var channel = ConfigService.Get(this.channel);

      Process p = new Process();
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.FileName = $"/etc/spotnik/{channel.RestartFile}";
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      p.WaitForExit();
    }

    private void ReadLog()
    {
      readlog = true;

      var wh = new AutoResetEvent(false);
      var fsw = new FileSystemWatcher(".");
      fsw.Filter = "file-to-read";
      fsw.EnableRaisingEvents = true;
      fsw.Changed += (s, e) => wh.Set();

      var fs = new FileStream("/tmp/svxlink.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      using (var sr = new StreamReader(fs))
      {
        var s = "";
        while (readlog)
        {
          s = sr.ReadLine();
          if (s != null)
            Console.WriteLine($"toto:{s}");
          else
            wh.WaitOne(1000);
        }
      }

      wh.Close();
    }

  }

}
