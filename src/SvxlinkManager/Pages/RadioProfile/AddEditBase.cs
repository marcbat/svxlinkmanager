using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public abstract class AddEditBase : RepositoryComponentBase
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Models.RadioProfile RadioProfile { get; set; }

    protected abstract void HandleValidSubmit();

    protected abstract string SubmitTitle { get; }
  }
}
