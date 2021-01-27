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

      SvxLinkService.TempChanged += SvxLinkService_TempChanged;

      SvxLinkService.TempoQsy += SvxLinkService_TempoQsy;

      SvxLinkService.Scanning += SvxLinkService_Scanning;

      SvxLinkService.ScanningQsy += SvxLinkService_ScanningQsy;
    }

    private async void SvxLinkService_TempoQsy()
    {
      try
      {
        await ShowInfoToastAsync("QSY", "Vous avez été redirigé sur le salon principal par la temporisation.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour d'afficher le toast. {e.Message}");
      }
    }

    private async void SvxLinkService_ScanningQsy()
    {
      try
      {
        await ShowInfoToastAsync("QSY", "Vous avez été redirigé par le scanner.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour d'afficher le toast. {e.Message}");
      }
    }

    private async void SvxLinkService_Scanning()
    {
      try
      {
        if (Scanning)
          return;

        Scanning = true;

        await ShowInfoToastAsync("Scan", "Le scan a débuté.");

        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur du scannning. {e.Message}");
      }
    }

    private void SvxLinkService_TempChanged(double timer)
    {
      try
      {
        TimerStatus = Convert.ToInt32(timer);

        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur du timer status. {e.Message}");
      }
    }

    public List<Channel> Channels { get; set; }

    private void LoadChannels() => Channels = Repositories.Channels.GetAll().ToList();

    private async void SvxLinkService_Error(string t, string b)
    {
      try
      {
        await ShowErrorToastAsync(t, b);
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour d'afficher le toast. {e.Message}");
      }
    }

    private void SvxLinkService_NodeRx(Models.Node n)
    {
      try
      {
        CurrentTxNode = null;
        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de repasser le node en RX. {e.Message}");
      }
    }

    private void SvxLinkService_NodeTx(Models.Node n)
    {
      try
      {
        CurrentTxNode = n;
        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de passer le node en TX. {e.Message}");
      }
    }

    private async void SvxLinkService_NodeDisconnected(Models.Node n)
    {
      try
      {
        await InvokeAsync(() => StateHasChanged());
        await ShowInfoToastAsync(n.Name, "A quitté le salon.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible d'indiquer qu'un noeus a quitté le salon. {e.Message}");
      }
    }

    private async void SvxLinkService_NodeConnected(Models.Node n)
    {
      try
      {
        await InvokeAsync(() => StateHasChanged());
        await ShowInfoToastAsync(n.Name, "A rejoint le salon.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible d'indiquer qu'un noeud a rejoint le salon. {e.Message}");
      }
    }

    private void SvxLinkService_Disconnected()
    {
      try
      {
        CurrentTxNode = null;
        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible d'indiquer l'état déconnecté. {e.Message}");
      }
    }

    private async void SvxLinkService_ConnectedAsync(Channel c)
    {
      try
      {
        TimerStatus = 0;
        Scanning = false;

        await InvokeAsync(() => StateHasChanged());
        await ShowSuccessToastAsync("Connecté", $"Vous êtes maintenant connecté au salon:<br/><strong>{c.Name}</strong>");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour d'afficher le toast. {e.Message}");
      }
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

    public int TimerStatus { get; set; } = 0;

    public bool Scanning { get; set; } = false;

    public List<Models.Node> Nodes
    {
      get => SvxLinkService.Nodes.OrderBy(n => n.Name).ToList();
      set => SvxLinkService.Nodes = value;
    }
  }
}