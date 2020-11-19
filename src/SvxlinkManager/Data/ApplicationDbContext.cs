using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SvxlinkManager.Models;

namespace SvxlinkManager.Data
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

    public DbSet<Rule> Rules { get; set; }

    public DbSet<RadioProfile> RadioProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=Spotnik.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });

      builder.Entity<Channel>().HasData(new Channel { Id = 1, Name = "Réseau des Répéteurs Francophones", CallSign= "(CH) HB9GXP2 H", ReportCallSign="HB9GXP", Host= "rrf2.f5nlg.ovh", Port = 5300,  AuthKey= "Magnifique123456789!", IsDefault=true, IsTemporized = false, Dtmf = 96 });
      builder.Entity<Channel>().HasData(new Channel { Id = 2, Name = "Salon Suisse Romand", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "salonsuisseromand.northeurope.cloudapp.azure.com", Port = 5300, AuthKey = "xD9wW5gO7yD9hN5o", IsDefault = false, IsTemporized = true, Dtmf = 104 });
      builder.Entity<Channel>().HasData(new Channel { Id = 3, Name = "French Open Network", CallSign= "(CH) HB9GXP2 H", ReportCallSign="HB9GXP", Host= "serveur.f1tzo.com", Port= 5300, AuthKey= "FON-F1TZO" , IsDefault = false, IsTemporized = true, Dtmf = 97 });
      builder.Entity<Channel>().HasData(new Channel { Id = 4, Name = "Salon Technique", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "rrf3.f5nlg.ovh", Port = 5301, AuthKey = "Magnifique123456789!" , IsDefault = false, IsTemporized = true, Dtmf = 98 });
      builder.Entity<Channel>().HasData(new Channel { Id = 5, Name = "Salon International", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "rrf3.f5nlg.ovh", Port = 5302, AuthKey = "Magnifique123456789!" , IsDefault = false, IsTemporized = true, Dtmf = 99 });
      builder.Entity<Channel>().HasData(new Channel { Id = 6, Name = "Salon Bavardage", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "serveur.f1tzo.com", Port = 5301, AuthKey = "FON-F1TZO" , IsDefault = false, IsTemporized = true, Dtmf = 100 });
      builder.Entity<Channel>().HasData(new Channel { Id = 7, Name = "Salon Local", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "serveur.f1tzo.com", Port= 5302, AuthKey= "FON-F1TZO" , IsDefault = false, IsTemporized = true, Dtmf = 101 });
      builder.Entity<Channel>().HasData(new Channel { Id = 8, Name = "Salon Expérimental", CallSign = "(CH) HB9GXP2 H", ReportCallSign = "HB9GXP", Host = "rrf3.f5nlg.ovh", Port = 5303, AuthKey = "Magnifique123456789!" , IsDefault = false, IsTemporized = true, Dtmf = 102 });

      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 1, Name = "VHF défaut", TxFrequ = "144.700", RxFequ = "144.700", Squelch = "2", RxCtcss = "0002", Enable= true });
      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 2, Name = "UHF défaut", TxFrequ = "436.375", RxFequ = "436.375", Squelch = "2", RxCtcss = "0005" });
    }
  }
}
