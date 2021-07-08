using SvxlinkManager.Data;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public interface IParameterRepository
  {
    string GetStringValue(string key);

    int GetInt32Value(string key);
  }

  public class ParameterRepository : Repository<SvxlinkManagerParameter>, IParameterRepository
  {
    public ParameterRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public string GetStringValue(string key)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      var parameter = dbcontext.Parameters.Single(p => p.Key == key);

      return parameter.Value;
    }

    public int GetInt32Value(string key)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      var parameter = dbcontext.Parameters.Single(p => p.Key == key);

      return Int32.Parse(parameter.Value);
    }
  }
}