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

    TEntity FindBy(Func<TEntity, bool> predicate);

    TEntity Get(int id);

    TEntity GetBy(Func<TEntity, bool> predicate);

    IEnumerable<TEntity> GetAll();

    IEnumerable<TEntity> GetAllBy(Func<TEntity, bool> predicate);

    TEntity Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(int id);
  }

  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModelEntity
  {
    protected readonly Data.IDbContextFactory<ApplicationDbContext> contextFactory;

    public Repository(Data.IDbContextFactory<ApplicationDbContext> contextFactory)
    {
      this.contextFactory = contextFactory;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().ToList();
    }

    public IEnumerable<TEntity> GetAllBy(Func<TEntity, bool> predicate)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().Where(predicate).ToList();
    }

    public virtual TEntity Get(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().Single(e => e.Id == id);
    }

    public TEntity GetBy(Func<TEntity, bool> predicate)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().Single(predicate);
    }

    public virtual TEntity Find(int id)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().SingleOrDefault(e => e.Id == id);
    }

    public TEntity FindBy(Func<TEntity, bool> predicate)
    {
      using var dbcontext = contextFactory.CreateDbContext();
      return dbcontext.Set<TEntity>().SingleOrDefault(predicate);
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