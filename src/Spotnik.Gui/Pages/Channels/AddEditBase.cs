using Microsoft.AspNetCore.Components;
using Spotnik.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Pages.Channels
{
  public class AddEditBase : RepositoryComponentBase
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Channel Channel { get; set; }
  }
}
