using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class AdvanceChannel : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "AdvanceSvxlinkChannels",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            SvxlinkConf = table.Column<string>(type: "TEXT", nullable: true),
            ModuleDtmfRepeater = table.Column<string>(type: "TEXT", nullable: true),
            ModuleEchoLink = table.Column<string>(type: "TEXT", nullable: true),
            ModuleFrn = table.Column<string>(type: "TEXT", nullable: true),
            ModuleHelp = table.Column<string>(type: "TEXT", nullable: true),
            ModuleMetarInfo = table.Column<string>(type: "TEXT", nullable: true),
            ModuleParrot = table.Column<string>(type: "TEXT", nullable: true),
            ModulePropagationMonitor = table.Column<string>(type: "TEXT", nullable: true),
            ModuleSelCallEnc = table.Column<string>(type: "TEXT", nullable: true),
            ModuleTclVoiceMail = table.Column<string>(type: "TEXT", nullable: true),
            ModuleTrx = table.Column<string>(type: "TEXT", nullable: true),
            Name = table.Column<string>(type: "TEXT", nullable: false),
            Dtmf = table.Column<int>(type: "INTEGER", nullable: false),
            IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
            IsTemporized = table.Column<bool>(type: "INTEGER", nullable: false),
            TimerDelay = table.Column<int>(type: "INTEGER", nullable: false),
            SoundId = table.Column<int>(type: "INTEGER", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AdvanceSvxlinkChannels", x => x.Id);
            table.ForeignKey(
                      name: "FK_AdvanceSvxlinkChannels_Sound_SoundId",
                      column: x => x.SoundId,
                      principalTable: "Sound",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_AdvanceSvxlinkChannels_SoundId",
          table: "AdvanceSvxlinkChannels",
          column: "SoundId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "AdvanceSvxlinkChannels");
    }
  }
}