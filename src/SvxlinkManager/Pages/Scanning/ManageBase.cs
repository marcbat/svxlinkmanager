using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Scanning
{
  [Authorize]
  public class ManageBase : RepositoryComponentBase
  {
    public EditContext EditContext;

    protected override async Task OnInitializedAsync()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Scan page") { Url = new Uri("/Scanning/Manage", UriKind.Relative) });

      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadScanProfile();
      LoadChannels();

      EditContext = new EditContext(ScanProfile);
      EditContext.OnFieldChanged += (s, e) => IsChanged = true;
    }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    private void LoadScanProfile()
    {
      ScanProfile = Repositories.ScanProfiles.Get(1);
    }

    private void LoadChannels()
    {
      Channels = Repositories.Channels.GetAll().Where(c => !string.IsNullOrEmpty(c.TrackerUrl)).ToList();
    }

    public bool IsChanged { get; set; }

    protected bool IsChecked(Channel channel) => ScanProfile.Channels.Contains(channel);

    public ScanProfile ScanProfile { get; set; }

    protected async Task EnableScan()
    {
      ScanProfile.Enable = true;

      Repositories.ScanProfiles.Update(ScanProfile);

      Telemetry.TrackEvent("Enable scan", ScanProfile.TrackProperties);

      SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

      await ShowSuccessToastAsync("Activé", $"le scan a bien été activé.");
    }

    protected async Task DisableScan()
    {
      ScanProfile.Enable = false;

      Repositories.ScanProfiles.Update(ScanProfile);

      Telemetry.TrackEvent("Disable scan", ScanProfile.TrackProperties);

      SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

      await ShowSuccessToastAsync("Désactivé", $"le scan a bien été désactivé.");
    }

    protected void AddRemoveChannel(Channel channel, ChangeEventArgs e)
    {
      if ((bool)e.Value)
        ScanProfile.Channels.Add(channel);
      else
        ScanProfile.Channels.Remove(channel);

      IsChanged = true;
    }

    public List<Channel> Channels { get; protected set; }

    protected async Task HandleValidSubmitAsync()
    {
      Repositories.ScanProfiles.Update(ScanProfile);

      Telemetry.TrackEvent("Update scan profile", ScanProfile.TrackProperties);

      SvxLinkService.ActivateChannel(SvxLinkService.ChannelId);

      IsChanged = false;

      await ShowSuccessToastAsync("Modifié", $"le scan profil {ScanProfile.Name} a bien été modifié.");
    }
  }
}