using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using Spotnik.Gui.Repositories;
using Spotnik.Gui.Service;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spotnik.Gui.ViewModels
{
  public class HomeBase : ComponentBase, INotifyPropertyChanged
  {
    private int channel;
    private string status = "Déconnecté";
    private List<string> nodes = new List<string>();

    public event PropertyChangedEventHandler PropertyChanged;

    protected override void OnInitialized()
    {
      base.OnInitialized();

      Nodes = new List<string>();

      LoadChannels();

      PropertyChanged += (s, e) =>
      {
        if (e.PropertyName == nameof(Channel))
          RunRestart();
      };

      SwxLinkLogService.Connected += ns =>
      {
        Nodes = ns;
        StateHasChanged();
      };

      SwxLinkLogService.NodeConnected += n =>
      {
        Nodes.Add(n);
        StateHasChanged();
      };

      SwxLinkLogService.NodeDisconnected += n =>
      {
        Nodes.Remove(n);
        StateHasChanged();
      };
    }

    
    [Inject]
    public IRepositories Repositories { get; set; }

    [Inject]
    public SwxLinkLogService SwxLinkLogService { get; set; }

    [Inject]
    public ILogger<HomeBase> Logger { get; set; }

    public string Status
    {
      get => status;
      set
      {
        status = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
      }
    }

    public int Channel
    {
      get => channel;
      set
      {
        channel = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Channel)));
      }
    }

    public List<Channel> Channels { get; private set; }

    public List<string> Nodes { get; set; }

    private void LoadChannels() => Channels = Repositories.Channels.GetAll().ToList();

    private void RunRestart()
    {
      Logger.LogInformation("Restart salon.");

      // Arrete de lire les log
      SwxLinkLogService.StopRead();
      Logger.LogInformation("Arret de la lecture des logs");

      // Stop svxlink
      var pid = ExecuteCommand("pgrep -x svxlink");
      if(pid!=null)
        ExecuteCommand("pkill -TERM svxlink");

      Status = "Déconnecté";
      Logger.LogInformation("Salon déconnecté");

      // Si le choix est Déconnecter, fin de la méthode
      if (Channel == 0)
        return;
      
      // Récupère le channel
      var channel = Repositories.Channels.Get(Channel);
      Logger.LogInformation($"Recupération du salon {channel.Name}");

      // Remplace le contenu de svxlink.conf avec le informations du channel
      ReplaceConfig(channel);
      Logger.LogInformation("Remplacement du contenu svxlink.current");

      // Vide les logs
      File.WriteAllText("/tmp/svxlink.log", string.Empty);
      Logger.LogInformation("Les logs sont vidés");

      // Commence à lire les logs
      SwxLinkLogService.StartReadAsync();
      Logger.LogInformation("Commencer à lire les logs");

      // Lance svxlink
      //ExecuteCommand("svxlink --daemon --logfile=/tmp/svxlink.log --pidfile=/var/run/svxlink.pid --runasuser=root --config=/etc/spotnik/svxlink.current");
      ExecuteCommand("svxlink --daemon --pidfile=/var/run/svxlink.pid --runasuser=root --config=/etc/spotnik/svxlink.current");
      Status = "Connecté";
      Logger.LogInformation($"Le channel {channel.Name} est connecté.");
    }

    private string ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      var process = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      process.WaitForExit();

      return result.Trim();
    }

    private void ReplaceConfig(Channel channel)
    {
      File.Copy("/etc/spotnik/svxlink.conf", "/etc/spotnik/svxlink.current", true);

      string text = File.ReadAllText("/etc/spotnik/svxlink.current");
      text = text.Replace("HOST=HOST", $"HOST={channel.Host}");
      text = text.Replace("AUTH_KEY=AUTH_KEY", $"AUTH_KEY={channel.AuthKey}");
      text = text.Replace("PORT=PORT", $"PORT={channel.Port}");
      text = text.Replace("CALLSIGN=CALLSIGN", $"CALLSIGN={channel.CallSign}");
      File.WriteAllText("/etc/spotnik/svxlink.current", text);
    }

  }

}
