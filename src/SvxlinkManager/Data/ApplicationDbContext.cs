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
    #region Constructors

    public ApplicationDbContext() : base()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #endregion Constructors

    #region Properties

    public DbSet<Channel> Channels { get; set; }

    public DbSet<RadioProfile> RadioProfiles { get; set; }

    public DbSet<Rule> Rules { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=Spotnik.db");
    }

    /// <summary>
    /// Configures the schema needed for svxlink manager application. Seed the primary data.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });

      builder.Entity<Channel>().HasData(new Channel { Id = 1, Name = "Réseau des Répéteurs Francophones", SoundName = "Srrf.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!", IsDefault = true, IsTemporized = false, Dtmf = 96 });
      builder.Entity<Channel>().HasData(new Channel { Id = 2, Name = "Salon Suisse Romand", SoundName = "Sreg.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "salonsuisseromand.northeurope.cloudapp.azure.com", Port = 5300, AuthKey = "xD9wW5gO7yD9hN5o", IsDefault = false, IsTemporized = true, Dtmf = 104 });
      builder.Entity<Channel>().HasData(new Channel { Id = 3, Name = "French Open Network", SoundName = "Sfon.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5300, AuthKey = "FON-F1TZO", IsDefault = false, IsTemporized = true, Dtmf = 97 });
      builder.Entity<Channel>().HasData(new Channel { Id = 4, Name = "Salon Technique", SoundName = "Stec.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5301, AuthKey = "Magnifique123456789!", IsDefault = false, IsTemporized = true, Dtmf = 98 });
      builder.Entity<Channel>().HasData(new Channel { Id = 5, Name = "Salon International", SoundName = "Sint.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5302, AuthKey = "Magnifique123456789!", IsDefault = false, IsTemporized = true, Dtmf = 99 }); ;
      builder.Entity<Channel>().HasData(new Channel { Id = 6, Name = "Salon Bavardage", SoundName = "Sbav.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5301, AuthKey = "FON-F1TZO", IsDefault = false, IsTemporized = true, Dtmf = 100 });
      builder.Entity<Channel>().HasData(new Channel { Id = 7, Name = "Salon Local", SoundName = "Sloc.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5302, AuthKey = "FON-F1TZO", IsDefault = false, IsTemporized = true, Dtmf = 101 });
      builder.Entity<Channel>().HasData(new Channel { Id = 8, Name = "Salon Expérimental", SoundName = "Sexp.wav", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5303, AuthKey = "Magnifique123456789!", IsDefault = false, IsTemporized = true, Dtmf = 102 });

      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 1, Name = "VHF défaut", TxFrequ = "144.700", RxFequ = "144.700", Squelch = "2", RxCtcss = "0002", Enable = true });
      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 2, Name = "UHF défaut", TxFrequ = "436.375", RxFequ = "436.375", Squelch = "2", RxCtcss = "0005" });
    }

    #endregion Methods
  }
}