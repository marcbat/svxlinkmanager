using Spotnik.Gui.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public interface IRepositories
  {
    ChannelRepository Channels { get; }
  }

  public class Repositories : IRepositories
  {
    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new ChannelRepository(contextFactory);
    }

    public ChannelRepository Channels { get; private set; }

  }
}
