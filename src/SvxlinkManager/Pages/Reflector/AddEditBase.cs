using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
  public abstract class AddEditBase<TLocalizer> : RepositoryComponentBase<TLocalizer>
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);

      await Js.InvokeVoidAsync("SetEditor");
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    [Inject]
    public IIniService IniService { get; set; }

    protected Models.Reflector Reflector { get; set; }

    protected abstract Task HandleValidSubmitAsync();

    protected abstract string SubmitTitle { get; }
  }
}