﻿using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public interface IChannelRepository : IRepository<Channel>
  {
    Channel GetDefault();
  }

  public class ChannelRepository : Repository<Channel>, IChannelRepository
  {
    public ChannelRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override void Update(Channel channel)
    {
      base.Update(channel);

      // Set other channel to none default
      if (channel.IsDefault)
      {
        foreach (var c in GetAll().Where(c => !c.Id.Equals(channel.Id)))
        {
          c.IsDefault = false;
          Update(c);
        }
      }
    }

    public Channel GetDefault()
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Channels.Single(c => c.IsDefault);
    }
  }
}
