using SvxlinkManager.Data;
using SvxlinkManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public interface IRepositories
  {
    IChannelRepository Channels { get; }

    IRadioProfileRepository RadioProfiles { get; }
  }

  public class Repositories : IRepositories
  {
    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new ChannelRepository(contextFactory);
      RadioProfiles = new RadioProfileRepository(contextFactory);
    }

    public IChannelRepository Channels { get; private set; }

    public IRadioProfileRepository RadioProfiles { get; private set; }

  }
}
