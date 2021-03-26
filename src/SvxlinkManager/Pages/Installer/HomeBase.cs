using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
  public enum InstallationStatus
  {
    Security,
    Channel,
    DefaultChannel,
    RadioProfile,
    Wifi,
    Update
  }

  public class HomeBase : RepositoryComponentBase
  {
    protected override void OnInitialized()
    {
      base.OnInitialized();

      InstallerModel = new InstallerModel
      {
        Channels = LoadChannels()
      };
    }

    private List<SvxlinkChannel> LoadChannels() => Repositories.SvxlinkChannels.GetAll().ToList();

    public InstallerModel InstallerModel { get; set; }

    public InstallationStatus InstallationStatus { get; set; } = InstallationStatus.Security;
  }
}