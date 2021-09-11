using Microsoft.AspNetCore.Components;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public abstract class AddEditBase<TLocalizer> : RepositoryComponentBase<TLocalizer>
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Models.RadioProfile RadioProfile { get; set; }

    protected abstract Task HandleValidSubmitAsync();

    protected abstract string SubmitTitle { get; }
  }
}