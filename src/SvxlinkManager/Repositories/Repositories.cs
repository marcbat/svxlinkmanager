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

    IRepository<SvxlinkChannel> SvxlinkChannels { get; set; }

    IRepository<EcholinkChannel> EcholinkChannels { get; set; }

    IRepository<ScanProfile> ScanProfiles { get; set; }

    Repository<TEntity> Repository<TEntity>() where TEntity : class, IModelEntity;
  }

  public class Repositories : IRepositories
  {
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new ChannelRepository(contextFactory);
      RadioProfiles = new RadioProfileRepository(contextFactory);
      SvxlinkChannels = new Repository<SvxlinkChannel>(contextFactory);
      EcholinkChannels = new Repository<EcholinkChannel>(contextFactory);
      ScanProfiles = new ScanProfileRepository(contextFactory);
      this.contextFactory = contextFactory;
    }

    public IChannelRepository Channels { get; private set; }

    public IRadioProfileRepository RadioProfiles { get; private set; }

    public IRepository<SvxlinkChannel> SvxlinkChannels { get; set; }

    public IRepository<EcholinkChannel> EcholinkChannels { get; set; }

    public IRepository<ScanProfile> ScanProfiles { get; set; }

    public Repository<TEntity> Repository<TEntity>() where TEntity : class, IModelEntity => new Repository<TEntity>(contextFactory);
  }
}