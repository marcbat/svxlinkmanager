using Microsoft.AspNetCore.Components;
using Spotnik.Gui.Models;
using Spotnik.Gui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spotnik.Gui.ViewModels
{
  public class HomeBase : ComponentBase
  {
    private string salon;

    protected override void OnInitialized()
    {
      base.OnInitialized();

      LoadChannels();
    }

    [Inject]
    public IConfigService ConfigService { get; set; }

    public string Salon
    {
      get => salon;
      set {
        salon = value;

        RunRestart();
      }
    }

    public List<Channel> Salons { get; private set; }

    private void LoadChannels() => Salons = ConfigService.GetChannel();

    private void RunRestart()
    {
      var channel = ConfigService.Get(salon);

      Process p = new Process();
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.FileName = $"/etc/spotnik/{channel.RestartFile}";
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      p.WaitForExit();
    }

  }

}
