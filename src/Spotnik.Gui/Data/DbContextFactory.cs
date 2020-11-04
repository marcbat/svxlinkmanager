using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SvxlinkManager.Data
{

  public interface IDbContextFactory<TContext> where TContext : DbContext
  {
    /// <summary>
    /// Generate a new <see cref="DbContext"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="TContext"/>.</returns>
    TContext CreateDbContext();
  }

  public class DbContextFactory<TContext> 
        : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly IServiceProvider provider;

        public DbContextFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public TContext CreateDbContext()
        {
            if (provider == null)
            {
                throw new InvalidOperationException(
                    $"You must configure an instance of IServiceProvider");
            }

            return ActivatorUtilities.CreateInstance<TContext>(provider);
        }
    }
}
