using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using SvxlinkManager.Data;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Repositories;
using SvxlinkManager.Service;

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

namespace SvxlinkManager.Pages
{
  public class HomeBase : ChannelBase
  {
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      SvxLinkService.Connected += async () =>
      {
        await ShowToastAsync(Guid.NewGuid().ToString(), "Connecté", "Vous êtes maintenant connecté.", "success");
        await InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.Disconnected += () =>
      {
        CurrentTxNode = null;
        InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.NodeConnected += n =>
        InvokeAsync(() => StateHasChanged());

      SvxLinkService.NodeDisconnected += n =>
        InvokeAsync(() => StateHasChanged());

      SvxLinkService.NodeTx += n =>
      {
        CurrentTxNode = n;
        InvokeAsync(() => StateHasChanged());
      };

      SvxLinkService.NodeRx += n =>
      {
        CurrentTxNode = null;
        InvokeAsync(() => StateHasChanged());
      };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
    }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    public Models.Node CurrentTxNode { get; set; }

    public string Status
    {
      get => SvxLinkService.Status;
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