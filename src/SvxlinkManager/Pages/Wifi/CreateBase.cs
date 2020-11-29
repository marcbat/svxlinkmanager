using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  public class CreateBase : RepositoryComponentBase
  {
    public Connection Connection { get; private set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public WifiService WifiService { get; set; }

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit()
    {
      WifiService.AddConnection(Connection);

      NavigationManager.NavigateTo("Wifi/Manage");
    }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      Connection = new Connection();

      LoadSsid();
    }

    private void LoadSsid()
    {
    }
  }
}