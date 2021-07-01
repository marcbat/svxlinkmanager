using System;
using System.Collections.Generic;
using System.IO;
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

    public DbSet<ManagedChannel> Channels { get; set; }

    public DbSet<SvxlinkChannel> svxlinkChannels { get; set; }

    public DbSet<EcholinkChannel> EcholinkChannels { get; set; }

    public DbSet<AdvanceSvxlinkChannel> AdvanceSvxlinkChannels { get; set; }

    public DbSet<RadioProfile> RadioProfiles { get; set; }

    public DbSet<ScanProfile> ScanProfiles { get; set; }

    public DbSet<Rule> Rules { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=SvxlinkManager.db");
    }

    /// <summary>
    /// Configures the schema needed for svxlink manager application. Seed the primary data.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<ManagedChannel>()
        .HasOne(c => c.Sound)
        .WithOne(s => s.Channel)
        .HasForeignKey<Sound>(s => s.ChannelId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });

      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 1, Name = "Réseau des Répéteurs Francophones", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf2.f5nlg.ovh", Port = 5300, AuthKey = "Magnifique123456789!", IsTemporized = false, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/RRF-today/rrf_tiny.json", Dtmf = 96 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 2, Name = "Salon Suisse Romand", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "salonsuisseromand.northeurope.cloudapp.azure.com", Port = 5300, AuthKey = "xD9wW5gO7yD9hN5o", IsTemporized = true, TimerDelay = 180, Dtmf = 104 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 3, Name = "French Open Network", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5300, AuthKey = "FON-F1TZO", IsTemporized = true, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/FON-today/rrf_tiny.json", Dtmf = 97 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 4, Name = "Salon Technique", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5301, AuthKey = "Magnifique123456789!", IsTemporized = true, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/TECHNIQUE-today/rrf_tiny.json", Dtmf = 98 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 5, Name = "Salon International", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5302, AuthKey = "Magnifique123456789!", IsTemporized = true, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/INTERNATIONAL-today/rrf_tiny.json", Dtmf = 99 }); ;
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 6, Name = "Salon Bavardage", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5301, AuthKey = "FON-F1TZO", IsTemporized = true, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/BAVARDAGE-today/rrf_tiny.json", Dtmf = 100 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 7, Name = "Salon Local", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "serveur.f1tzo.com", Port = 5302, AuthKey = "FON-F1TZO", IsTemporized = true, TimerDelay = 180, TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/LOCAL-today/rrf_tiny.json", Dtmf = 101 });
      builder.Entity<SvxlinkChannel>().HasData(new SvxlinkChannel { Id = 8, Name = "Salon Expérimental", CallSign = "(CH) SVX4LINK H", ReportCallSign = "SVX4LINK", Host = "rrf3.f5nlg.ovh", Port = 5303, AuthKey = "Magnifique123456789!", IsTemporized = true, TimerDelay = 180, TrackerUrl = "", Dtmf = 102 });

      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 1, Name = "VHF défaut", TxFrequ = "144.700", RxFequ = "144.700", Squelch = "2", RxCtcss = "0002", SquelchDetection = "GPIO", HasSa818 = true, Enable = true });
      builder.Entity<RadioProfile>().HasData(new RadioProfile { Id = 2, Name = "UHF défaut", TxFrequ = "436.375", RxFequ = "436.375", Squelch = "2", RxCtcss = "0005", SquelchDetection = "GPIO", HasSa818 = true, Enable = false });

      builder.Entity<ScanProfile>().HasData(new ScanProfile { Id = 1, Name = "default", ScanDelay = 60, Enable = false });
    }

    #endregion Methods
  }
}