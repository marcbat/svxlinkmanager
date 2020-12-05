using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using SvxlinkManager.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages
{
  public enum ToastType
  {
    Success,
    info,
    Danger
  }

  public class RepositoryComponentBase : ComponentBase
  {
    [Inject]
    public ILogger<HomeBase> Logger { get; set; }

    [Inject]
    public IRepositories Repositories { get; set; }

    [Inject]
    public IJSRuntime Js { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);

      await Js.InvokeVoidAsync("SetToolTips");

      await Js.InvokeVoidAsync("SetPopOver");
    }

    /// <summary>Show new toast.</summary>
    /// <param name="title">Toast title</param>
    /// <param name="body">Toast body</param>
    /// <param name="type">Toast type. Accept : success, info, danger</param>
    /// <param name="autohide">Autohide, always false if type is danger</param>
    /// <param name="delay">Autohide delay in second</param>
    protected virtual async Task ShowToastAsync(string title, string body, ToastType type = ToastType.info, bool autohide = true, int delay = 5000)
    {
      string toastclass;

      switch (type)
      {
        case ToastType.Success:
          toastclass = "success";
          break;

        case ToastType.Danger:
          toastclass = "danger";
          autohide = false;
          break;

        default:
          toastclass = "info";
          break;
      }

      await Js.InvokeVoidAsync("addToast", Guid.NewGuid().ToString(), title, body, toastclass, DateTime.Now.ToString("HH:mm"), autohide, delay);
    }
  }
}