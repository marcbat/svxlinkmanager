using Microsoft.AspNetCore.Components;

using Spotnik.Gui.Data;
using Spotnik.Gui.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public class ChannelRepository
  {

    [Inject]
    public IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

    public IEnumerable<Channel> GetChannels()
    {
      using var dbcontext = DbFactory.CreateDbContext();
      return dbcontext.Channels.ToList();

    }
  }
}
