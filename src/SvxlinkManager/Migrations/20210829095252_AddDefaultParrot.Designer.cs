﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvxlinkManager.Data;

namespace SvxlinkManager.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210829095252_AddDefaultParrot")]
    partial class AddDefaultParrot
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                            Id = "e660ad64-dd6f-48e2-b585-e22ff2a42918",
                            ConcurrencyStamp = "dfa68f5f-d1fe-44dd-b62e-a723f7ee0ea3",
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

            modelBuilder.Entity("SvxlinkManager.Models.ManagedChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Dtmf")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTemporized")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ScanProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimerDelay")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TrackerUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ScanProfileId");

                    b.ToTable("Channels");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ManagedChannel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.RadioProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasSa818")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HightPass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LowPass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
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
                            HasSa818 = true,
                            HightPass = "0",
                            LowPass = "0",
                            Name = "VHF défaut",
                            PreEmph = "0",
                            RxCtcss = "0002",
                            RxFequ = "144.700",
                            Squelch = "2",
                            SquelchDetection = "GPIO",
                            TxCtcss = "0000",
                            TxFrequ = "144.700",
                            Volume = "4"
                        },
                        new
                        {
                            Id = 2,
                            Enable = false,
                            HasSa818 = true,
                            HightPass = "0",
                            LowPass = "0",
                            Name = "UHF défaut",
                            PreEmph = "0",
                            RxCtcss = "0005",
                            RxFequ = "436.375",
                            Squelch = "2",
                            SquelchDetection = "GPIO",
                            TxCtcss = "0000",
                            TxFrequ = "436.375",
                            Volume = "4"
                        });
                });

            modelBuilder.Entity("SvxlinkManager.Models.Reflector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Config")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Reflectors");
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

            modelBuilder.Entity("SvxlinkManager.Models.ScanProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ScanDelay")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("ScanProfiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Enable = false,
                            Name = "default",
                            ScanDelay = 60
                        });
                });

            modelBuilder.Entity("SvxlinkManager.Models.Sound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("SoundFile")
                        .HasColumnType("BLOB");

                    b.Property<string>("SoundName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.ToTable("Sound");
                });

            modelBuilder.Entity("SvxlinkManager.Models.SvxlinkManagerParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Parameters");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Key = "default.svxlink.conf",
                            Value = "[GLOBAL]\r\nLOGICS=LOGICS\r\nCFG_DIR = svxlink.d\r\nTIMESTAMP_FORMAT =% c\r\nCARD_SAMPLE_RATE = 16000\r\nCARD_CHANNELS=1\r\nLINKS=ALLlink\r\n\r\n[SimplexLogic]\r\nTYPE=Simplex\r\nRX=Rx1\r\nTX=Tx1\r\nMODULES=ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor,ModuleParrot\r\nCALLSIGN=REPORTCALLSIGN\r\nSHORT_IDENT_INTERVAL=15\r\nLONG_IDENT_INTERVAL=60\r\nIDENT_ONLY_AFTER_TX=10\r\nEXEC_CMD_ON_SQL_CLOSE=500\r\nEVENT_HANDLER=/usr/share/svxlink/events.tcl\r\nDEFAULT_LANG=fr_FR\r\nRGR_SOUND_ALWAYS=1\r\nRGR_SOUND_DELAY=0\r\nREPORT_CTCSS=REPORT_CTCSS\r\nTX_CTCSS=ALWAYS\r\nMACROS=Macros\r\nFX_GAIN_NORMAL=0\r\nFX_GAIN_LOW=-12\r\nACTIVATE_MODULE_ON_LONG_CMD=10:PropagationMonitor\r\nMUTE_RX_ON_TX=1\r\nDTMF_CTRL_PTY=/tmp/dtmf_uhf\r\n\r\n[ALLlink]\r\nCONNECT_LOGICS=SimplexLogic:434MHZ:945,ReflectorLogic\r\nDEFAULT_ACTIVE=1\r\nTIMEOUT=0\r\n\r\n[Rx1]\r\nTYPE=Local\r\nAUDIO_DEV=alsa:plughw:0\r\nAUDIO_CHANNEL=0\r\nSQL_DET=GPIO\r\nSQL_START_DELAY=500\r\nSQL_DELAY=100\r\nSQL_HANGTIME=20\r\nSQL_EXTENDED_HANGTIME=1000\r\nSQL_EXTENDED_HANGTIME_THRESH=13\r\nSQL_TIMEOUT=600\r\nVOX_FILTER_DEPTH=300\r\nVOX_THRESH=1000\r\nCTCSS_MODE=2\r\nCTCSS_FQ=71.9\r\nCTCSS_SNR_OFFSET=0\r\nCTCSS_OPEN_THRESH=15\r\nCTCSS_CLOSE_THRESH=9\r\nCTCSS_BPF_LOW=60\r\nCTCSS_BPF_HIGH=260\r\nGPIO_PATH=/sys/class/gpio\r\nGPIO_SQL_PIN=gpio10\r\nDEEMPHASIS=0\r\nSQL_TAIL_ELIM=0\r\nPREAMP=-4\r\nPEAK_METER=1\r\nDTMF_DEC_TYPE=INTERNAL\r\nDTMF_MUTING=1\r\nDTMF_HANGTIME=40\r\n1750_MUTING=1\r\n\r\n[Tx1]\r\nTYPE=Local\r\nAUDIO_DEV=alsa:plughw:0\r\nAUDIO_CHANNEL=0\r\nPTT_TYPE=GPIO\r\nGPIO_PATH=/sys/class/gpio\r\nPTT_PIN=gpio7\r\nTIMEOUT=300\r\nTX_DELAY=900\r\nPREAMP=0\r\nCTCSS_FQ=71.9\r\nCTCSS_LEVEL=9\r\nPREEMPHASIS=0\r\nDTMF_TONE_LENGTH=100\r\nDTMF_TONE_SPACING=50\r\nDTMF_DIGIT_PWR=-15\r\n\r\n[ReflectorLogic]\r\nTYPE=Reflector\r\nAUDIO_CODEC=OPUS\r\nJITTER_BUFFER_DELAY=2\r\nCALLSIGN=CALLSIGN\r\nHOST=HOST\r\nAUTH_KEY=AUTH_KEY\r\nPORT=PORT\r\n"
                        },
                        new
                        {
                            Id = 2,
                            Key = "default.echolink.conf",
                            Value = "[ModuleEchoLink]\r\nNAME=EchoLink\r\nID=2\r\nSERVERS=europe.echolink.org\r\nCALLSIGN=CALLSIGN\r\nPASSWORD=PASSWORD\r\nSYSOPNAME=SYSOPNAME\r\nLOCATION=LOCATION\r\nMAX_QSOS=4\r\nMAX_CONNECTIONS=5\r\nLINK_IDLE_TIMEOUT=300\r\nUSE_GSM_ONLY=0\r\nDESCRIPTION=DESCRIPTION\r\nDEFAULT_LANG=fr_FR\r\n"
                        },
                        new
                        {
                            Id = 3,
                            Key = "default.parrot.conf",
                            Value = "[ModuleParrot]\r\nNAME=Parrot\r\nID=1\r\nTIMEOUT=600\r\nFIFO_LEN=60\r\nREPEAT_DELAY=1000\r\n"
                        });
                });

            modelBuilder.Entity("SvxlinkManager.Models.AdvanceSvxlinkChannel", b =>
                {
                    b.HasBaseType("SvxlinkManager.Models.ManagedChannel");

                    b.Property<string>("ModuleDtmfRepeater")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleEchoLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleFrn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleHelp")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleMetarInfo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleParrot")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModulePropagationMonitor")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleSelCallEnc")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleTclVoiceMail")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleTrx")
                        .HasColumnType("TEXT");

                    b.Property<string>("SvxlinkConf")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("AdvanceSvxlinkChannel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.Channel", b =>
                {
                    b.HasBaseType("SvxlinkManager.Models.ManagedChannel");

                    b.Property<string>("CallSign")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Channel");
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
                            Dtmf = 96,
                            IsDefault = false,
                            IsTemporized = false,
                            Name = "Réseau des Répéteurs Francophones",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/RRF-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "rrf2.f5nlg.ovh",
                            AuthKey = "Magnifique123456789!",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 2,
                            Dtmf = 104,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Suisse Romand",
                            TimerDelay = 180,
                            CallSign = "(CH) SVX4LINK H",
                            Host = "salonsuisseromand.hbspot.ch",
                            AuthKey = "xD9wW5gO7yD9hN5o",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 3,
                            Dtmf = 97,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "French Open Network",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/FON-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "serveur.f1tzo.com",
                            AuthKey = "FON-F1TZO",
                            Port = 5300,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 4,
                            Dtmf = 98,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Technique",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/TECHNIQUE-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "rrf3.f5nlg.ovh",
                            AuthKey = "Magnifique123456789!",
                            Port = 5301,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 5,
                            Dtmf = 99,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon International",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/INTERNATIONAL-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "rrf3.f5nlg.ovh",
                            AuthKey = "Magnifique123456789!",
                            Port = 5302,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 6,
                            Dtmf = 100,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Bavardage",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/BAVARDAGE-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "serveur.f1tzo.com",
                            AuthKey = "FON-F1TZO",
                            Port = 5301,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 7,
                            Dtmf = 101,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Local",
                            TimerDelay = 180,
                            TrackerUrl = "http://rrf.f5nlg.ovh:8080/RRFTracker/LOCAL-today/rrf_tiny.json",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "serveur.f1tzo.com",
                            AuthKey = "FON-F1TZO",
                            Port = 5302,
                            ReportCallSign = "SVX4LINK"
                        },
                        new
                        {
                            Id = 8,
                            Dtmf = 102,
                            IsDefault = false,
                            IsTemporized = true,
                            Name = "Salon Expérimental",
                            TimerDelay = 180,
                            TrackerUrl = "",
                            CallSign = "(CH) SVX4LINK H",
                            Host = "rrf3.f5nlg.ovh",
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

            modelBuilder.Entity("SvxlinkManager.Models.ManagedChannel", b =>
                {
                    b.HasOne("SvxlinkManager.Models.ScanProfile", null)
                        .WithMany("Channels")
                        .HasForeignKey("ScanProfileId");
                });

            modelBuilder.Entity("SvxlinkManager.Models.Rule", b =>
                {
                    b.HasOne("SvxlinkManager.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.ScanProfile", b =>
                {
                    b.HasOne("SvxlinkManager.Models.Channel", null)
                        .WithMany("ScanProfiles")
                        .HasForeignKey("ChannelId");
                });

            modelBuilder.Entity("SvxlinkManager.Models.Sound", b =>
                {
                    b.HasOne("SvxlinkManager.Models.ManagedChannel", "Channel")
                        .WithOne("Sound")
                        .HasForeignKey("SvxlinkManager.Models.Sound", "ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("SvxlinkManager.Models.ManagedChannel", b =>
                {
                    b.Navigation("Sound");
                });

            modelBuilder.Entity("SvxlinkManager.Models.ScanProfile", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("SvxlinkManager.Models.Channel", b =>
                {
                    b.Navigation("ScanProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
