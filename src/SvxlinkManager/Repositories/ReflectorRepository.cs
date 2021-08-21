using SvxlinkManager.Data;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public class ReflectorRepository : Repository<Reflector>
  {
    public ReflectorRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override void Update(Reflector reflector)
    {
      base.Update(reflector);

      foreach (var r in GetAll().Where(r => !r.Id.Equals(reflector.Id)))
      {
        r.Enable = true;
        base.Update(r);
      }
    }
  }
}