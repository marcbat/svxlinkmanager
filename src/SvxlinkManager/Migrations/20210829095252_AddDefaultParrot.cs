using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
  public partial class AddDefaultParrot : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
          table: "Parameters",
          columns: new[] { "Id", "Key", "Value" },
          values: new object[] { 3, "default.parrot.conf", "[ModuleParrot]\r\nNAME=Parrot\r\nID=1\r\nTIMEOUT=600\r\nFIFO_LEN=60\r\nREPEAT_DELAY=1000\r\n" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
          table: "Parameters",
          keyColumn: "Id",
          keyValue: 3);
    }
  }
}