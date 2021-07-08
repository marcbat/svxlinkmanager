using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class AdvanceChannel : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "ChannelScanProfile");

      migrationBuilder.AddColumn<int>(
          name: "ChannelId",
          table: "ScanProfiles",
          type: "INTEGER",
          nullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Host",
          table: "Channels",
          type: "TEXT",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "TEXT");

      migrationBuilder.AlterColumn<string>(
          name: "CallSign",
          table: "Channels",
          type: "TEXT",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "TEXT");

      migrationBuilder.AddColumn<string>(
          name: "ModuleDtmfRepeater",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleEchoLink",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleFrn",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleHelp",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleMetarInfo",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleParrot",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModulePropagationMonitor",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleSelCallEnc",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleTclVoiceMail",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleTrx",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "ScanProfileId",
          table: "Channels",
          type: "INTEGER",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "SvxlinkConf",
          table: "Channels",
          type: "TEXT",
          nullable: true);

      migrationBuilder.CreateIndex(
          name: "IX_ScanProfiles_ChannelId",
          table: "ScanProfiles",
          column: "ChannelId");

      migrationBuilder.CreateIndex(
          name: "IX_Channels_ScanProfileId",
          table: "Channels",
          column: "ScanProfileId");

      migrationBuilder.AddForeignKey(
          name: "FK_Channels_ScanProfiles_ScanProfileId",
          table: "Channels",
          column: "ScanProfileId",
          principalTable: "ScanProfiles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_ScanProfiles_Channels_ChannelId",
          table: "ScanProfiles",
          column: "ChannelId",
          principalTable: "Channels",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Channels_ScanProfiles_ScanProfileId",
          table: "Channels");

      migrationBuilder.DropForeignKey(
          name: "FK_ScanProfiles_Channels_ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropIndex(
          name: "IX_ScanProfiles_ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropIndex(
          name: "IX_Channels_ScanProfileId",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropColumn(
          name: "ModuleDtmfRepeater",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleEchoLink",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleFrn",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleHelp",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleMetarInfo",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleParrot",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModulePropagationMonitor",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleSelCallEnc",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleTclVoiceMail",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ModuleTrx",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "ScanProfileId",
          table: "Channels");

      migrationBuilder.DropColumn(
          name: "SvxlinkConf",
          table: "Channels");

      migrationBuilder.AlterColumn<string>(
          name: "Host",
          table: "Channels",
          type: "TEXT",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "TEXT",
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "CallSign",
          table: "Channels",
          type: "TEXT",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "TEXT",
          oldNullable: true);

      migrationBuilder.CreateTable(
          name: "ChannelScanProfile",
          columns: table => new
          {
            ChannelsId = table.Column<int>(type: "INTEGER", nullable: false),
            ScanProfilesId = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ChannelScanProfile", x => new { x.ChannelsId, x.ScanProfilesId });
            table.ForeignKey(
                      name: "FK_ChannelScanProfile_Channels_ChannelsId",
                      column: x => x.ChannelsId,
                      principalTable: "Channels",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ChannelScanProfile_ScanProfiles_ScanProfilesId",
                      column: x => x.ScanProfilesId,
                      principalTable: "ScanProfiles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_ChannelScanProfile_ScanProfilesId",
          table: "ChannelScanProfile",
          column: "ScanProfilesId");
    }
  }
}