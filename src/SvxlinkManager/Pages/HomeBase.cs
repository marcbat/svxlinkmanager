using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Common.Queries;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages
{
    [Authorize]
    public class HomeBase : MediatrComponentBase, IDisposable
    {
        private Task<string> channel;

        protected override async Task OnInitializedAsync()
        {

            await LoadChannelsAsync();

            SvxLinkService.Connected += SvxLinkService_ConnectedAsync;

            SvxLinkService.Disconnected += SvxLinkService_Disconnected;

            SvxLinkService.NodeConnected += SvxLinkService_NodeConnected;

            SvxLinkService.NodeDisconnected += SvxLinkService_NodeDisconnected;

            SvxLinkService.NodeTx += SvxLinkService_NodeTx;

            SvxLinkService.NodeRx += SvxLinkService_NodeRx;

            SvxLinkService.Error += SvxLinkService_Error;

            //SvxLinkService.StopTempo += SvxLinkService_StopTempo;

            //SvxLinkService.StartTempo += SvxLinkService_StartTempo;

            //SvxLinkService.TempChanged += SvxLinkService_TempChanged;

            //SvxLinkService.TempoQsy += SvxLinkService_TempoQsy;

            //SvxLinkService.Scanning += SvxLinkService_Scanning;

            //SvxLinkService.StopScanning += SvxLinkService_StopScanning;

            //SvxLinkService.ScanningQsy += SvxLinkService_ScanningQsy;
        }

        private void SvxLinkService_Connected2(Domain.Entities.ChannelBase obj)
        {
            throw new NotImplementedException();
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
            }
        }

        public List<ManagedChannel> Channels { get; set; } = new List<ManagedChannel>();

        private async Task LoadChannelsAsync()
        {
            foreach (var channel in await Mediatr.Send(new GetAllManagedChannelQuery(Options.Value.ConfigId)))
            {
                switch (channel)
                {
                    case Domain.Entities.SvxlinkChannel svxlinkChannel:
                        Channels.Add((SvxlinkChannel)svxlinkChannel);
                        break;
                    case Domain.Entities.AdvanceSvxlinkChannel advanceSvxlinkChannel:
                        Channels.Add((AdvanceSvxlinkChannel)advanceSvxlinkChannel);
                        break;
                    default:
                        throw new Exception("Channel type not supported");
                }

            }
        }

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

        private void SvxLinkService_NodeRx(Domain.Entities.Node n)
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

        private void SvxLinkService_NodeTx(Domain.Entities.Node n)
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

        private async void SvxLinkService_NodeDisconnected(Domain.Entities.Node n)
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

        private async void SvxLinkService_NodeConnected(Domain.Entities.Node n)
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
                Scanning = false;
                InvokeAsync(() => StateHasChanged());
            }
            catch (Exception e)
            {
                Logger.LogError($"Impossible d'indiquer l'état déconnecté. {e.Message}");
            }
        }

        private async void SvxLinkService_ConnectedAsync(Domain.Entities.ChannelBase c)
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

            //SvxLinkService.StopTempo -= SvxLinkService_StopTempo;

            //SvxLinkService.StartTempo -= SvxLinkService_StartTempo;

            //SvxLinkService.TempChanged -= SvxLinkService_TempChanged;

            //SvxLinkService.TempoQsy -= SvxLinkService_TempoQsy;

            //SvxLinkService.Scanning -= SvxLinkService_Scanning;

            //SvxLinkService.StopScanning -= SvxLinkService_StopScanning;

            //SvxLinkService.ScanningQsy -= SvxLinkService_ScanningQsy;
        }

        [Inject]
        public ISvxlinkServiceBase SvxLinkService { get; set; }

        public Models.Node CurrentTxNode { get; set; }

        public string Status
        {
            get => SvxLinkService.Status;
        }

        public Guid? ActiveChannel
        {
            get => SvxLinkService.ActiveChannel;
        }

        public async Task<string> Channel
        {
            get => channel; 
            set
            {
                channel = value;
                await Mediatr.Send(new ActivateSvxlinkChannelCommand(Options.Value.ConfigId, Guid.Parse(value)));
            }
        }

        public string TemporizationValue { get; set; }

        public bool TemporizationIsActive { get; set; } = false;

        public bool Scanning { get; set; } = false;

        public List<Models.Node> Nodes { get; set; } = new List<Models.Node>();
        //{
        //    get => SvxLinkService.Nodes.OrderBy(n => n.Name).ToList();
        //    set => SvxLinkService.Nodes = value;
        //}
    }
}