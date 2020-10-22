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
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public ChannelRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      this.contextFactory = contextFactory;
    }

    public IEnumerable<Channel> GetChannels()
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Channels.ToList();

    }
  }
}
