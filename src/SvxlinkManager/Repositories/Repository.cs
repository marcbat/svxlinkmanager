using Microsoft.EntityFrameworkCore;
using SvxlinkManager.Data;
using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
  public interface IRepository<TEntity> where TEntity : class, IModelEntity
  {
    TEntity Find(int id);
    TEntity Get(int id);
    IEnumerable<TEntity> GetAll();

    TEntity Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(int id);
  }

  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModelEntity
  {
    protected readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public Repository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      this.contextFactory = contextFactory;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().ToList();
    }

    public virtual TEntity Get(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().Single(e => e.Id == id);
    }

    public virtual TEntity Find(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().SingleOrDefault(e => e.Id == id);
    }

    public virtual TEntity Add(TEntity entity)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      dbcontext.Set<TEntity>().Add(entity);
      dbcontext.SaveChanges();

      return entity;
    }

    public virtual void Update(TEntity entity)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      dbcontext.Set<TEntity>().Attach(entity);

      dbcontext.Entry(entity).State = EntityState.Modified;

      dbcontext.SaveChanges();
    }

    public virtual void Delete(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();

      var entity = Get(id);
      dbcontext.Attach(entity);
      dbcontext.Set<TEntity>().Remove(entity);

      dbcontext.SaveChanges();

    }
  }
}
