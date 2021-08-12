using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using SvxlinkManager.Common.Models;

namespace SvxlinkManager.Pages.Shared
{
  public class SvxlinkManagerComponentBase : ComponentBase
  {
    [Inject]
    public ILogger<SvxlinkManagerComponentBase> Logger { get; set; }

    [Inject]
    public TelemetryClient Telemetry { get; set; }

    [Inject]
    public IJSRuntime Js { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);

      await Js.InvokeVoidAsync("SetToolTips");

      await Js.InvokeVoidAsync("SetPopOver");
    }

    protected async Task ShowErrorToastAsync(string title, string body, bool autohide = true, int delay = 10000) =>
      await ShowToastAsync(title, body, "danger", autohide, delay);

    protected async Task ShowInfoToastAsync(string title, string body, bool autohide = true, int delay = 5000) =>
      await ShowToastAsync(title, body, "info", autohide, delay);

    protected async Task ShowSuccessToastAsync(string title, string body, bool autohide = true, int delay = 5000) =>
      await ShowToastAsync(title, body, "success", autohide, delay);

    /// <summary>Show new toast.</summary>
    /// <param name="title">Toast title</param>
    /// <param name="body">Toast body</param>
    /// <param name="type">Toast type. Accept : success, info, danger</param>
    /// <param name="autohide">Autohide, always false if type is danger</param>
    /// <param name="delay">Autohide delay in second</param>
    private async Task ShowToastAsync(string title, string body, string type, bool autohide = true, int delay = 5000) =>
      await Js.InvokeVoidAsync("addToast", Guid.NewGuid().ToString(), title, body, type, DateTime.Now.ToString("HH:mm:ss"), autohide, delay);

    protected virtual (string, string) ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      Logger.LogInformation($"Execution de la commande {cmd}.");

      var process = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      string error = process.StandardError.ReadToEnd();
      process.WaitForExit();

      return (result?.Trim(), error?.Trim());
    }

    public virtual void Replace<TEntity>(List<TEntity> collection, TEntity entity) where TEntity : IModelEntity
    {
      int index = collection.FindIndex(r => r.Id == entity.Id);

      collection.RemoveAt(index);

      collection.Insert(index, entity);
    }
  }
}