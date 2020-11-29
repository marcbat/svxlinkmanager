using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Host = table.Column<string>(type: "TEXT", nullable: false),
                    AuthKey = table.Column<string>(type: "TEXT", nullable: true),
                    Port = table.Column<int>(type: "INTEGER", nullable: false),
                    CallSign = table.Column<string>(type: "TEXT", nullable: false),
                    ReportCallSign = table.Column<string>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTemporized = table.Column<bool>(type: "INTEGER", nullable: false),
                    Dtmf = table.Column<int>(type: "INTEGER", nullable: false),
                    SoundName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadioProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    RxFequ = table.Column<string>(type: "TEXT", nullable: true),
                    TxFrequ = table.Column<string>(type: "TEXT", nullable: true),
                    Squelch = table.Column<string>(type: "TEXT", nullable: true),
                    TxCtcss = table.Column<string>(type: "TEXT", nullable: true),
                    RxCtcss = table.Column<string>(type: "TEXT", nullable: true),
                    Volume = table.Column<string>(type: "TEXT", nullable: true),
                    PreEmph = table.Column<string>(type: "TEXT", nullable: true),
                    HightPass = table.Column<string>(type: "TEXT", nullable: true),
                    LowPass = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadioProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "32682d52-b892-4877-aed2-a8cd42077960", "9cae3b52-c005-4422-90ed-157b5837ebfb", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 1, "Magnifique123456789!", "(CH) SVX4LINK H", 96, "rrf2.f5nlg.ovh", true, false, "Réseau des Répéteurs Francophones", 5300, "SVX4LINK", "Srrf.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 2, "xD9wW5gO7yD9hN5o", "(CH) SVX4LINK H", 104, "salonsuisseromand.northeurope.cloudapp.azure.com", false, true, "Salon Suisse Romand", 5300, "SVX4LINK", "Sreg.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 3, "FON-F1TZO", "(CH) SVX4LINK H", 97, "serveur.f1tzo.com", false, true, "French Open Network", 5300, "SVX4LINK", "Sfon.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 4, "Magnifique123456789!", "(CH) SVX4LINK H", 98, "rrf3.f5nlg.ovh", false, true, "Salon Technique", 5301, "SVX4LINK", "Stec.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 5, "Magnifique123456789!", "(CH) SVX4LINK H", 99, "rrf3.f5nlg.ovh", false, true, "Salon International", 5302, "SVX4LINK", "Sint.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 6, "FON-F1TZO", "(CH) SVX4LINK H", 100, "serveur.f1tzo.com", false, true, "Salon Bavardage", 5301, "SVX4LINK", "Sbav.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 7, "FON-F1TZO", "(CH) SVX4LINK H", 101, "serveur.f1tzo.com", false, true, "Salon Local", 5302, "SVX4LINK", "Sloc.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 8, "Magnifique123456789!", "(CH) SVX4LINK H", 102, "rrf3.f5nlg.ovh", false, true, "Salon Expérimental", 5303, "SVX4LINK", "Sexp.wav" });

            migrationBuilder.InsertData(
                table: "RadioProfiles",
                columns: new[] { "Id", "Enable", "HightPass", "LowPass", "Name", "PreEmph", "RxCtcss", "RxFequ", "Squelch", "TxCtcss", "TxFrequ", "Volume" },
                values: new object[] { 1, true, "0", "0", "VHF défaut", "0", "0002", "144.700", "2", "0000", "144.700", "4" });

            migrationBuilder.InsertData(
                table: "RadioProfiles",
                columns: new[] { "Id", "Enable", "HightPass", "LowPass", "Name", "PreEmph", "RxCtcss", "RxFequ", "Squelch", "TxCtcss", "TxFrequ", "Volume" },
                values: new object[] { 2, false, "0", "0", "UHF défaut", "0", "0005", "436.375", "2", "0000", "436.375", "4" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rules_ChannelId",
                table: "Rules",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RadioProfiles");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}
