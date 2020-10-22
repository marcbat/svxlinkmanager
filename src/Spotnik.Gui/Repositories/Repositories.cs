using Spotnik.Gui.Data;
using Spotnik.Gui.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public interface IRepositories
  {
    IRepository<Channel> Channels { get; }
  }

  public class Repositories : IRepositories
  {
    public Repositories(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      Channels = new Repository<Channel>(contextFactory);
    }

    public IRepository<Channel> Channels { get; private set; }

  }
}
