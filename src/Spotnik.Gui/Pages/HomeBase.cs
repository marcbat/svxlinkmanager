using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using Spotnik.Gui.Pages.Shared;
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

namespace Spotnik.Gui.Pages
{
  public class HomeBase : ChannelBase, INotifyPropertyChanged
  {
    private string status = "Déconnecté";

    public event PropertyChangedEventHandler PropertyChanged;

    protected override async Task OnInitializedAsync()
    {

      await base.OnInitializedAsync().ConfigureAwait(false);

      SvxLinkService.Connected += () =>
      {
        Status = "Connectés";
        InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.Disconnected += () =>
      {
        Status = "Déconnecté";
        InvokeAsync(() => StateHasChanged());
      };
        
      SvxLinkService.NodeConnected += n =>
        InvokeAsync(() => StateHasChanged());

      SvxLinkService.NodeDisconnected += n =>
        InvokeAsync(() => StateHasChanged());
      
      SvxLinkService.NodeTx += n =>
        InvokeAsync(() => StateHasChanged());
      
      SvxLinkService.NodeRx += n =>
        InvokeAsync(() => StateHasChanged());
      
    }

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
      get => SvxLinkService.Channel;
      set => SvxLinkService.Channel = value;
    }

    public List<Models.Node> Nodes
    {
      get => SvxLinkService.Nodes;
      set => SvxLinkService.Nodes = value;
    }

  }

}
