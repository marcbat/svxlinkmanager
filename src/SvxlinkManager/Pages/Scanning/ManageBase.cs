using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Scanning
{
  public class ManageBase : RepositoryComponentBase
  {
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadScanProfile();

      LoadChannels();
    }

    private void LoadScanProfile()
    {
      ScanProfile = Repositories.ScanProfiles.Get(1);
    }

    private void LoadChannels()
    {
      Channels = Repositories.Channels.GetAll().Where(c => !string.IsNullOrEmpty(c.TrackerUrl)).ToList();
    }

    public ScanProfile ScanProfile { get; set; }

    protected async Task EnableScan()
    {
      ScanProfile.Enable = true;

      Repositories.ScanProfiles.Update(ScanProfile);

      await ShowSuccessToastAsync("Activé", $"le scan a bien été activé.");
    }

    protected async Task DisableScan()
    {
      ScanProfile.Enable = false;

      Repositories.ScanProfiles.Update(ScanProfile);

      await ShowSuccessToastAsync("Désactivé", $"le scan a bien été désactivé.");
    }

    protected void AddRemoveChannel(Channel channel, ChangeEventArgs e)
    {
      if ((bool)e.Value)
        ScanProfile.Channels.Add(channel);
      else
        ScanProfile.Channels.Remove(channel);
    }

    public List<Channel> Channels { get; protected set; }

    protected async Task HandleValidSubmitAsync()
    {
      Repositories.ScanProfiles.Update(ScanProfile);

      await ShowSuccessToastAsync("Modifié", $"le scan profil {ScanProfile.Name} a bien été modifié.");
    }
  }
}