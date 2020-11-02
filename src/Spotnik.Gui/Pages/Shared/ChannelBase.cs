using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Spotnik.Gui.Models;
using Spotnik.Gui.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Pages.Shared
{
  public class ChannelBase : RepositoryComponentBase
  {

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadChannels();
    }

    
    public List<Channel> Channels { get; set; }

    private void LoadChannels() => Channels = Repositories.Channels.GetAll().ToList();

  }
}
