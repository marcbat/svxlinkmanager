using Microsoft.EntityFrameworkCore;

using SvxlinkManager.Data;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public class ScanProfileRepository : Repository<ScanProfile>
  {
    public ScanProfileRepository(Data.IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public override ScanProfile Get(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.ScanProfiles.Include(s => s.Channels).Single(e => e.Id == id);
    }

    public override void Update(ScanProfile scanProfile)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      var existingScanProfile = dbcontext.ScanProfiles.Include(s => s.Channels).Single(e => e.Id == scanProfile.Id);

      var toAdd = scanProfile.Channels.Where(c => !existingScanProfile.Channels.Contains(c)).ToList();

      foreach (var channel in toAdd)
        existingScanProfile.Channels.Add(channel);

      var toDelete = existingScanProfile.Channels.Where(c => !scanProfile.Channels.Contains(c)).ToList();

      foreach (var channel in toDelete)
        existingScanProfile.Channels.Remove(channel);

      existingScanProfile.ScanDelay = scanProfile.ScanDelay;

      dbcontext.SaveChanges();
    }
  }
}