using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class AdvanceChannel : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Rules_Channels_ChannelId",
          table: "Rules");

      migrationBuilder.DropForeignKey(
          name: "FK_Sound_Channels_ChannelId",
          table: "Sound");

      migrationBuilder.DropTable(
          name: "ChannelScanProfile");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Channels",
          table: "Channels");

      migrationBuilder.RenameTable(
          name: "Channels",
          newName: "ManagedChannel");

      migrationBuilder.AddColumn<int>(
          name: "ChannelId",
          table: "ScanProfiles",
          type: "INTEGER",
          nullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Host",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "TEXT");

      migrationBuilder.AlterColumn<string>(
          name: "CallSign",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "TEXT");

      migrationBuilder.AddColumn<string>(
          name: "ModuleDtmfRepeater",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleEchoLink",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleFrn",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleHelp",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleMetarInfo",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleParrot",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModulePropagationMonitor",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleSelCallEnc",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleTclVoiceMail",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ModuleTrx",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "ScanProfileId",
          table: "ManagedChannel",
          type: "INTEGER",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "SvxlinkConf",
          table: "ManagedChannel",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddPrimaryKey(
          name: "PK_ManagedChannel",
          table: "ManagedChannel",
          column: "Id");

      migrationBuilder.CreateIndex(
          name: "IX_ScanProfiles_ChannelId",
          table: "ScanProfiles",
          column: "ChannelId");

      migrationBuilder.CreateIndex(
          name: "IX_ManagedChannel_ScanProfileId",
          table: "ManagedChannel",
          column: "ScanProfileId");

      migrationBuilder.AddForeignKey(
          name: "FK_ManagedChannel_ScanProfiles_ScanProfileId",
          table: "ManagedChannel",
          column: "ScanProfileId",
          principalTable: "ScanProfiles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_Rules_ManagedChannel_ChannelId",
          table: "Rules",
          column: "ChannelId",
          principalTable: "ManagedChannel",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_ScanProfiles_ManagedChannel_ChannelId",
          table: "ScanProfiles",
          column: "ChannelId",
          principalTable: "ManagedChannel",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_Sound_ManagedChannel_ChannelId",
          table: "Sound",
          column: "ChannelId",
          principalTable: "ManagedChannel",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_ManagedChannel_ScanProfiles_ScanProfileId",
          table: "ManagedChannel");

      migrationBuilder.DropForeignKey(
          name: "FK_Rules_ManagedChannel_ChannelId",
          table: "Rules");

      migrationBuilder.DropForeignKey(
          name: "FK_ScanProfiles_ManagedChannel_ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropForeignKey(
          name: "FK_Sound_ManagedChannel_ChannelId",
          table: "Sound");

      migrationBuilder.DropIndex(
          name: "IX_ScanProfiles_ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropPrimaryKey(
          name: "PK_ManagedChannel",
          table: "ManagedChannel");

      migrationBuilder.DropIndex(
          name: "IX_ManagedChannel_ScanProfileId",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ChannelId",
          table: "ScanProfiles");

      migrationBuilder.DropColumn(
          name: "ModuleDtmfRepeater",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleEchoLink",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleFrn",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleHelp",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleMetarInfo",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleParrot",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModulePropagationMonitor",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleSelCallEnc",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleTclVoiceMail",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ModuleTrx",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "ScanProfileId",
          table: "ManagedChannel");

      migrationBuilder.DropColumn(
          name: "SvxlinkConf",
          table: "ManagedChannel");

      migrationBuilder.RenameTable(
          name: "ManagedChannel",
          newName: "Channels");

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

      migrationBuilder.AddPrimaryKey(
          name: "PK_Channels",
          table: "Channels",
          column: "Id");

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

      migrationBuilder.AddForeignKey(
          name: "FK_Rules_Channels_ChannelId",
          table: "Rules",
          column: "ChannelId",
          principalTable: "Channels",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_Sound_Channels_ChannelId",
          table: "Sound",
          column: "ChannelId",
          principalTable: "Channels",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }
  }
}