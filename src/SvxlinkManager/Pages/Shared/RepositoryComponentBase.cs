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

    protected virtual async Task ShowToastAsync(string id, string title, string body, string type, bool autohide = true, int delay = 5000)
    {
      if (type == "danger")
        autohide = false;

      await Js.InvokeVoidAsync("addToast", id, title, body, type, DateTime.Now.ToString("HH:mm"), autohide, delay);
    }
  }
}