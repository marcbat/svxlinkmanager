using Microsoft.EntityFrameworkCore;

using SvxlinkManager.Data;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public interface IChannelRepository : IRepository<Channel>
  {
    Channel GetDefault();

    Channel GetWithSound(int id);
  }

  public class ChannelRepository : Repository<Channel>, IChannelRepository
  {
    public ChannelRepository(Data.IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override Channel Add(Channel channel)
    {
      if (channel.IsDefault)
      {
        foreach (var c in GetAll())
        {
          c.IsDefault = false;
          Update(c);
        }
      }

      return base.Add(channel);
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

    public override void Delete(int id)
    {
      var channel = Get(id);
      if (channel.IsDefault)
        return;

      base.Delete(id);
    }

    public Channel GetDefault()
    {
      using (var dbcontext = contextFactory.CreateDbContext())
      {
        var defaut = dbcontext.Channels.SingleOrDefault(c => c.IsDefault);
        if (defaut != null)
          return defaut;
      }

      return SetFirstAsDefault();
    }

    private Channel SetFirstAsDefault()
    {
      Channel first;

      using (var dbcontext = contextFactory.CreateDbContext())
      {
        first = dbcontext.Channels.Find(1);
        first.IsDefault = true;
      }

      Update(first);

      return first;
    }

    public Channel GetWithSound(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Channels.Include(c => c.Sound).Single(e => e.Id == id);
    }
  }
}