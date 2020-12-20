﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvxlinkManager.Data;

namespace SvxlinkManager.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "ec395f1f-0493-4f42-bf0d-5f2edd139292",
                            ConcurrencyStamp = "4d9427b6-65dd-44ac-8ebe-801a332ae0a9",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SvxlinkManager.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CallSign")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Dtmf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTemporized")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SoundName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Channels");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Channel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.RadioProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HightPass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LowPass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreEmph")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RxCtcss")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RxFequ")
                        .HasColumnType("TEXT");

                    b.Property<string>("Squelch")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SquelchDetection")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TxCtcss")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TxFrequ")
                        .HasColumnType("TEXT");

                    b.Property<string>("Volume")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RadioProfiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Enable = true,
                            HightPass = "0",
                            LowPass = "0",
                            Name = "VHF défaut",
                            PreEmph = "0",
                            RxCtcss = "0002",
                            RxFequ = "144.700",
                            Squelch = "2",
                            SquelchDetection = "CTCSS",
                            TxCtcss = "0000",
                            TxFrequ = "144.700",
                            Volume = "4"
                        },
                        new
                        {
                            Id = 2,
                            Enable = false,
                            HightPass = "0",
                            LowPass = "0",
                            Name = "UHF défaut",
                            PreEmph = "0",
                            RxCtcss = "0005",
                            RxFequ = "436.375",
                            Squelch = "2",
                            SquelchDetection = "CTCSS",
                            TxCtcss = "0000",
                            TxFrequ = "436.375",
                            Volume = "4"
                        });
                });

            modelBuilder.Entity("SvxlinkManager.Models.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("SvxlinkManager.Models.EcholinkChannel", b =>
                {
                    b.HasBaseType("SvxlinkManager.Models.Channel");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxQso")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SysopName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("EcholinkChannel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.SvxlinkChannel", b =>
                {
                    b.HasBaseType("SvxlinkManager.Models.Channel");

                    b.Property<string>("AuthKey")
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReportCallSign")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("SvxlinkChannel");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 96,
                            Host = "rrf2.f5nlg.ovh",
                            IsDefault = false,
                            IsTemporized = false,
                            Name = "Réseau des Répéteurs Francophones",
                            SoundName = "Srrf.wav",
                            AuthKey = "Magnifique123456789!",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 2,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 104,
                            Host = "salonsuisseromand.northeurope.cloudapp.azure.com",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Suisse Romand",
                            SoundName = "Sreg.wav",
                            AuthKey = "xD9wW5gO7yD9hN5o",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 3,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 97,
                            Host = "serveur.f1tzo.com",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "French Open Network",
                            SoundName = "Sfon.wav",
                            AuthKey = "FON-F1TZO",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 4,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 98,
                            Host = "rrf3.f5nlg.ovh",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Technique",
                            SoundName = "Stec.wav",
                            AuthKey = "Magnifique123456789!",
                            Port = 5301,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 5,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 99,
                            Host = "rrf3.f5nlg.ovh",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon International",
                            SoundName = "Sint.wav",
                            AuthKey = "Magnifique123456789!",
                            Port = 5302,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 6,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 100,
                            Host = "serveur.f1tzo.com",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Bavardage",
                            SoundName = "Sbav.wav",
                            AuthKey = "FON-F1TZO",
                            Port = 5301,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 7,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 101,
                            Host = "serveur.f1tzo.com",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Local",
                            SoundName = "Sloc.wav",
                            AuthKey = "FON-F1TZO",
                            Port = 5302,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 8,
                            CallSign = "(CH) SVX4LINK H",
                            Dtmf = 102,
                            Host = "rrf3.f5nlg.ovh",
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Expérimental",
                            SoundName = "Sexp.wav",
                            AuthKey = "Magnifique123456789!",
                            Port = 5303,
                            ReportCallSign = "SVX4LINK"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SvxlinkManager.Models.Rule", b =>
                {
                    b.HasOne("SvxlinkManager.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId");

                    b.Navigation("Channel");
                });
#pragma warning restore 612, 618
        }
    }
}
