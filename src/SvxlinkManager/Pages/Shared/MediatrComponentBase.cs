using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Shared
{
  public class MediatrComponentBase : SvxlinkManagerComponentBase
  {
    [Inject]
    public IMediator Mediatr { get; set; }
  }
}