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
  public class HomeBase : RepositoryComponentBase, IDisposable
  {
    protected override async Task OnInitializedAsync()
    {
      LoadChannels();

      SvxLinkService.Connected += SvxLinkService_ConnectedAsync;

      SvxLinkService.Disconnected += SvxLinkService_Disconnected;

      SvxLinkService.NodeConnected += SvxLinkService_NodeConnected;

      SvxLinkService.NodeDisconnected += SvxLinkService_NodeDisconnected;

      SvxLinkService.NodeTx += SvxLinkService_NodeTx;

      SvxLinkService.NodeRx += SvxLinkService_NodeRx;

      SvxLinkService.Error += SvxLinkService_Error;
    }

    public List<Channel> Channels { get; set; }

    private void LoadChannels() => Channels = Repositories.Channels.GetAll().ToList();

    private async void SvxLinkService_Error(string t, string b)
    {
      await ShowErrorToastAsync(t, b);
    }

    private void SvxLinkService_NodeRx(Models.Node n)
    {
      CurrentTxNode = null;
      InvokeAsync(() => StateHasChanged());
    }

    private void SvxLinkService_NodeTx(Models.Node n)
    {
      CurrentTxNode = n;
      InvokeAsync(() => StateHasChanged());
    }

    private async void SvxLinkService_NodeDisconnected(Models.Node n)
    {
      await ShowInfoToastAsync(n.Name, "A quitté le salon.");
      await InvokeAsync(() => StateHasChanged());
    }

    private async void SvxLinkService_NodeConnected(Models.Node n)
    {
      await ShowInfoToastAsync(n.Name, "A rejoint le salon.");
      await InvokeAsync(() => StateHasChanged());
    }

    private void SvxLinkService_Disconnected()
    {
      CurrentTxNode = null;
      InvokeAsync(() => StateHasChanged());
    }

    private async void SvxLinkService_ConnectedAsync(Channel c)
    {
      await ShowSuccessToastAsync("Connecté", $"Vous êtes maintenant connecté au salon:<br/><strong>{c.Name}</strong>");
      await InvokeAsync(() => StateHasChanged());
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
    }

    public void Dispose()
    {
      SvxLinkService.Connected -= SvxLinkService_ConnectedAsync;

      SvxLinkService.Disconnected -= SvxLinkService_Disconnected;

      SvxLinkService.NodeConnected -= SvxLinkService_NodeConnected;

      SvxLinkService.NodeDisconnected -= SvxLinkService_NodeDisconnected;

      SvxLinkService.NodeTx -= SvxLinkService_NodeTx;

      SvxLinkService.NodeRx -= SvxLinkService_NodeRx;

      SvxLinkService.Error -= SvxLinkService_Error;
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
      get => SvxLinkService.ChannelId;
      set => SvxLinkService.ChannelId = value;
    }

    public List<Models.Node> Nodes
    {
      get => SvxLinkService.Nodes;
      set => SvxLinkService.Nodes = value;
    }
  }
}