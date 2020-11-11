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
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Host = table.Column<string>(nullable: false),
                    AuthKey = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    CallSign = table.Column<string>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    IsTemporized = table.Column<bool>(nullable: false),
                    Dtmf = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadioProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Enable = table.Column<bool>(nullable: false),
                    RxFequ = table.Column<string>(nullable: true),
                    TxFrequ = table.Column<string>(nullable: true),
                    Squelch = table.Column<string>(nullable: true),
                    TxCtcss = table.Column<string>(nullable: true),
                    RxCtcss = table.Column<string>(nullable: true),
                    Volume = table.Column<string>(nullable: true),
                    PreEmph = table.Column<string>(nullable: true),
                    HightPass = table.Column<string>(nullable: true),
                    LowPass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadioProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enable = table.Column<bool>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    ChannelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fde26f3f-85da-4fb2-8268-c5519b49832c", "8fc3aace-ea2a-4ebb-95f8-2d3058209049", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 1, "Magnifique123456789!", "(CH) HB9GXP2 H", 0, "rrf2.f5nlg.ovh", true, false, "Réseau des Répéteurs Francophones", 5300 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 2, "xD9wW5gO7yD9hN5o", "(CH) HB9GXP2 H", 0, "salonsuisseromand.northeurope.cloudapp.azure.com", false, true, "Salon Suisse Romand", 5300 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 3, "FON-F1TZO", "(CH) HB9GXP2 H", 0, "serveur.f1tzo.com", false, true, "French Open Network", 5300 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 4, "Magnifique123456789!", "(CH) HB9GXP2 H", 0, "rrf3.f5nlg.ovh", false, true, "Salon Technique", 5301 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 5, "Magnifique123456789!", "(CH) HB9GXP2 H", 0, "rrf3.f5nlg.ovh", false, true, "Salon International", 5302 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 6, "FON-F1TZO", "(CH) HB9GXP2 H", 0, "serveur.f1tzo.com", false, true, "Salon Bavardage", 5301 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 7, "FON-F1TZO", "(CH) HB9GXP2 H", 0, "serveur.f1tzo.com", false, true, "Salon Local", 5302 });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port" },
                values: new object[] { 8, "Magnifique123456789!", "(CH) HB9GXP2 H", 0, "rrf3.f5nlg.ovh", false, true, "Salon Expérimental", 5303 });

            migrationBuilder.InsertData(
                table: "RadioProfiles",
                columns: new[] { "Id", "Enable", "HightPass", "LowPass", "Name", "PreEmph", "RxCtcss", "RxFequ", "Squelch", "TxCtcss", "TxFrequ", "Volume" },
                values: new object[] { 1, false, "0", "0", "VHF défaut", "0", "0002", "144.700", "2", "0000", "144.700", "4" });

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
