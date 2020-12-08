using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using SvxlinkManager.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Shared
{
  public class RepositoryComponentBase : SvxlinkManagerComponentBase
  {
    [Inject]
    public IRepositories Repositories { get; set; }
  }
}