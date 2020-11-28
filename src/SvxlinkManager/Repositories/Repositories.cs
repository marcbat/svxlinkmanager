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
    #region Properties

    IChannelRepository Channels { get; }

    IRadioProfileRepository RadioProfiles { get; }

    IRepository<WifiConnection> WifiConnections { get; }

    #endregion Properties
  }

  public class Repositories : IRepositories
  {
    #region Constructors

    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new ChannelRepository(contextFactory);
      RadioProfiles = new RadioProfileRepository(contextFactory);
      WifiConnections = new Repository<WifiConnection>(contextFactory);
    }

    #endregion Constructors

    #region Properties

    public IChannelRepository Channels { get; private set; }

    public IRadioProfileRepository RadioProfiles { get; private set; }

    public IRepository<WifiConnection> WifiConnections { get; private set; }

    #endregion Properties
  }
}