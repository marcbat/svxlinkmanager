using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class advancechannel : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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

      migrationBuilder.AddColumn<string>(
          name: "SvxlinkConf",
          table: "Channels",
          type: "TEXT",
          nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
          name: "SvxlinkConf",
          table: "Channels");
    }
  }
}