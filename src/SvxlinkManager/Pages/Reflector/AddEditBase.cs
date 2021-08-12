using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
  public abstract class AddEditBase : RepositoryComponentBase
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);

      await Js.InvokeVoidAsync("SetEditor");
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Models.Reflector Reflector { get; set; }

    protected abstract Task HandleValidSubmitAsync();

    protected abstract string SubmitTitle { get; }
  }
}