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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spotnik.Gui.ViewModels
{
  public class HomeBase : ComponentBase, INotifyPropertyChanged
  {
    private int channel;
    private string status = "Déconnecté";

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

      SvxLinkService.Connected += async ns =>
      {
        Nodes = ns;
        await InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.NodeConnected += async n =>
      {
        Nodes.Add(n);
        await InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.NodeDisconnected += async n =>
      {
        Nodes.Remove(n);
        await InvokeAsync(() => StateHasChanged());
      };
    }

    
    [Inject]
    public IRepositories Repositories { get; set; }

    [Inject]
    public ILogger<HomeBase> Logger { get; set; }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

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

      // Stop svxlink
      SvxLinkService.StopSvxlink();
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

      // Lance svxlink
      SvxLinkService.RunsvxLink();
      Status = "Connecté";
      Logger.LogInformation($"Le channel {channel.Name} est connecté.");
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
