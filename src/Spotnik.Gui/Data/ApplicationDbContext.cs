using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Spotnik.Gui.Models;

namespace Spotnik.Gui.Data
{

  public class ApplicationDbContext : IdentityDbContext
  {
    public ApplicationDbContext() : base()
    {

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Channel> Channels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=Spotnik.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Channel>().HasData(new Channel { Id = 1, Name = "Réseau des Répéteurs Francophones" });
      builder.Entity<Channel>().HasData(new Channel { Id = 2, Name = "French Open Network" });
      builder.Entity<Channel>().HasData(new Channel { Id = 3, Name = "Salon Technique" });
      builder.Entity<Channel>().HasData(new Channel { Id = 4, Name = "Salon International" });
      builder.Entity<Channel>().HasData(new Channel { Id = 5, Name = "Salon Bavardage" });
      builder.Entity<Channel>().HasData(new Channel { Id = 6, Name = "Salon Local" });
      builder.Entity<Channel>().HasData(new Channel { Id = 7, Name = "Salon Expérimental" });
      builder.Entity<Channel>().HasData(new Channel { Id = 8, Name = "Réseau EchoLink" });
    }
  }
}
