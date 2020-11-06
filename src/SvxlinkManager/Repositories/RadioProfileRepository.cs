using SvxlinkManager.Data;
using SvxlinkManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public class RadioProfileRepository : Repository<RadioProfile>
  {
    public RadioProfileRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override void Update(RadioProfile profile)
    {
      base.Update(profile);

      if (profile.Enable)
      {
        foreach (var p in GetAll().Where(p => !p.Id.Equals(profile.Id)))
        {
          p.Enable = false;
          Update(p);
        }
      }
    }
  }
}
