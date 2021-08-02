using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class AddReflector : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Reflectors",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Name = table.Column<string>(type: "TEXT", nullable: true),
            Config = table.Column<string>(type: "TEXT", nullable: true),
            Enable = table.Column<bool>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Reflectors", x => x.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Reflectors");
    }
  }
}