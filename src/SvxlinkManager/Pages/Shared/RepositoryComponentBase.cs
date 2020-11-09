using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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
  }
}
