using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Models;

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  public class CreateBase : RepositoryComponentBase
  {
    #region Properties

    public WifiConnection Connection { get; private set; }

    #endregion Properties

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit()
    {
      Repositories.WifiConnections.Add(Connection);

      NavigationManager.NavigateTo("Wifi/Manage");
    }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      Connection = new WifiConnection();

      LoadSsid();
    }

    private void LoadSsid()
    {
      ExecuteCommand("nmcli device wifi list");
    }

    private void ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      var shell = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          StandardOutputEncoding = Encoding.UTF8,
          RedirectStandardError = true,
          StandardErrorEncoding = Encoding.UTF8,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };

      Action<object, DataReceivedEventArgs> logConsole = (s, e) => { Logger.LogInformation(e.Data); };

      shell.EnableRaisingEvents = true;
      shell.ErrorDataReceived += new DataReceivedEventHandler(logConsole);
      shell.OutputDataReceived += new DataReceivedEventHandler(logConsole);

      shell.Start();
      shell.BeginErrorReadLine();
      shell.BeginOutputReadLine();
    }
  }
}