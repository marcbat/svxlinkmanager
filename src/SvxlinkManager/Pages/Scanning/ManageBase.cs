using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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
    public EditContext EditContext;

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadScanProfile();
      LoadChannels();

      EditContext = new EditContext(ScanProfile);
      EditContext.OnFieldChanged += (s, e) => IsChanged = true;
    }

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

      IsChanged = true;
    }

    public List<Channel> Channels { get; protected set; }

    protected async Task HandleValidSubmitAsync()
    {
      Repositories.ScanProfiles.Update(ScanProfile);

      IsChanged = false;

      await ShowSuccessToastAsync("Modifié", $"le scan profil {ScanProfile.Name} a bien été modifié.");
    }
  }
}