using SvxlinkManager.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public interface IRepositories
  {
    IChannelRepository Channels { get; }
  }

  public class Repositories : IRepositories
  {
    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new ChannelRepository(contextFactory);
    }

    public IChannelRepository Channels { get; private set; }

  }
}
