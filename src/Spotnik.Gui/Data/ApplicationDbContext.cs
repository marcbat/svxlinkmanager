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

      builder.Entity<Channel>().HasData(new Channel { Id = 1, Name = "Réseau des Répéteurs Francophones", CallSign= "(CH) HB9GXP2 H", Host= "rrf2.f5nlg.ovh", Port = 5300,  AuthKey= "Magnifique123456789!" });
      builder.Entity<Channel>().HasData(new Channel { Id = 2, Name = "Salon Suisse Romand", CallSign = "(CH) HB9GXP2 H", Host = "salonsuisseromand.northeurope.cloudapp.azure.com", Port = 5300, AuthKey = "xD9wW5gO7yD9hN5o" });
      builder.Entity<Channel>().HasData(new Channel { Id = 3, Name = "French Open Network" });
      builder.Entity<Channel>().HasData(new Channel { Id = 4, Name = "Salon Technique", CallSign = "(CH) HB9GXP2 H", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!" });
      builder.Entity<Channel>().HasData(new Channel { Id = 5, Name = "Salon International", CallSign = "(CH) HB9GXP2 H", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!" });
      builder.Entity<Channel>().HasData(new Channel { Id = 6, Name = "Salon Bavardage", CallSign = "(CH) HB9GXP2 H", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!" });
      builder.Entity<Channel>().HasData(new Channel { Id = 7, Name = "Salon Local" });
      builder.Entity<Channel>().HasData(new Channel { Id = 8, Name = "Salon Expérimental", CallSign = "(CH) HB9GXP2 H", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!" });
      builder.Entity<Channel>().HasData(new Channel { Id = 9, Name = "Réseau EchoLink" });
    }
  }
}
