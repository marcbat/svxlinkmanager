using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
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
  [Authorize]
  public class HomeBase : RepositoryComponentBase<HomeBase>, IDisposable
  {
    protected override async Task OnInitializedAsync()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Accueil Page") { Url = new Uri("/", UriKind.Relative) });

      LoadChannels();

      SvxLinkService.Connected += SvxLinkService_ConnectedAsync;

      SvxLinkService.Disconnected += SvxLinkService_Disconnected;

      SvxLinkService.NodeConnected += SvxLinkService_NodeConnected;

      SvxLinkService.NodeDisconnected += SvxLinkService_NodeDisconnected;

      SvxLinkService.NodeTx += SvxLinkService_NodeTx;

      SvxLinkService.NodeRx += SvxLinkService_NodeRx;

      SvxLinkService.Error += SvxLinkService_Error;

      SvxLinkService.StopTempo += SvxLinkService_StopTempo;

      SvxLinkService.StartTempo += SvxLinkService_StartTempo;

      SvxLinkService.TempChanged += SvxLinkService_TempChanged;

      SvxLinkService.TempoQsy += SvxLinkService_TempoQsy;

      SvxLinkService.Scanning += SvxLinkService_Scanning;

      SvxLinkService.StopScanning += SvxLinkService_StopScanning;

      SvxLinkService.ScanningQsy += SvxLinkService_ScanningQsy;
    }

    private void SvxLinkService_StartTempo()
    {
      try
      {
        TemporizationIsActive = true;

        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur TemporizationIsActive. {e.Message}");
        Telemetry.TrackException(e);
      }
    }

    private void SvxLinkService_StopTempo()
    {
      try
      {
        TemporizationIsActive = false;

        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur TemporizationIsActive. {e.Message}");
        Telemetry.TrackException(e);
      }
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
        Telemetry.TrackException(e);
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
        Telemetry.TrackException(e);
      }
    }

    private async void SvxLinkService_StopScanning()
    {
      try
      {
        if (!Scanning)
          return;

        Scanning = false;

        await InvokeAsync(() => StateHasChanged());

        await ShowInfoToastAsync("Scan", "Le scan a été suspendu.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur du scannning. {e.Message}");
        Telemetry.TrackException(e);
      }
    }

    private async void SvxLinkService_Scanning()
    {
      try
      {
        if (Scanning)
          return;

        Scanning = true;

        await InvokeAsync(() => StateHasChanged());

        await ShowInfoToastAsync("Scan", "Le scan a débuté.");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur du scannning. {e.Message}");
        Telemetry.TrackException(e);
      }
    }

    private void SvxLinkService_TempChanged(string timer)
    {
      try
      {
        Logger.LogInformation($"La valeur de compte à rebour a changé. {timer}");

        TemporizationValue = timer;

        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour la valeur du timer status. {e.Message}");
        Telemetry.TrackException(e);
      }
    }

    public List<ManagedChannel> Channels { get; set; }

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
        Telemetry.TrackException(e);
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
        Telemetry.TrackException(e);
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
        Telemetry.TrackException(e);
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
        Telemetry.TrackException(e);
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
        Telemetry.TrackException(e);
      }
    }

    private void SvxLinkService_Disconnected()
    {
      try
      {
        CurrentTxNode = null;
        Scanning = false;
        InvokeAsync(() => StateHasChanged());
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible d'indiquer l'état déconnecté. {e.Message}");
        Telemetry.TrackException(e);
      }
    }

    private async void SvxLinkService_ConnectedAsync(ChannelBase c)
    {
      try
      {
        TemporizationValue = string.Empty;
        Scanning = false;

        await InvokeAsync(() => StateHasChanged());
        await ShowSuccessToastAsync("Connecté", $"Vous êtes maintenant connecté au salon:<br/><strong>{c.Name}</strong>");
      }
      catch (Exception e)
      {
        Logger.LogError($"Impossible de mettre à jour d'afficher le toast. {e.Message}");
        Telemetry.TrackException(e);
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

      SvxLinkService.StopTempo -= SvxLinkService_StopTempo;

      SvxLinkService.StartTempo -= SvxLinkService_StartTempo;

      SvxLinkService.TempChanged -= SvxLinkService_TempChanged;

      SvxLinkService.TempoQsy -= SvxLinkService_TempoQsy;

      SvxLinkService.Scanning -= SvxLinkService_Scanning;

      SvxLinkService.StopScanning -= SvxLinkService_StopScanning;

      SvxLinkService.ScanningQsy -= SvxLinkService_ScanningQsy;
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

    public string TemporizationValue { get; set; }

    public bool TemporizationIsActive { get; set; } = false;

    public bool Scanning { get; set; } = false;

    public List<Models.Node> Nodes
    {
      get => SvxLinkService.Nodes.OrderBy(n => n.Name).ToList();
      set => SvxLinkService.Nodes = value;
    }
  }
}