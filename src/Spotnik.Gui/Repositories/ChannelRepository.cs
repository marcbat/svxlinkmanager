using Spotnik.Gui.Data;
using Spotnik.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public class ChannelRepository : Repository<Channel>
  {
    public ChannelRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override void Update(Channel channel)
    {
      base.Update(channel);

      // Set other channel to none default
      if(channel.IsDefault)
      {
        foreach (var c in GetAll().Where(c=>!c.Id.Equals(channel.Id)))
        {
          c.IsDefault = false;
          Update(c);
        }
      }
    }
  }
}
