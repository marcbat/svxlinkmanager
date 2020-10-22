﻿using Spotnik.Gui.Data;
using Spotnik.Gui.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Repositories
{
  public interface IRepository<TEntity> where TEntity : class, IModelEntity
  {
    TEntity Find(int id);
    TEntity Get(int id);
    IEnumerable<TEntity> GetAll();
  }

  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModelEntity
  {
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public Repository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      this.contextFactory = contextFactory;
    }

    public IEnumerable<TEntity> GetAll()
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().ToList();
    }

    public TEntity Get(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().Single(e => e.Id == id);
    }

    public TEntity Find(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().SingleOrDefault(e => e.Id == id);
    }
  }
}
