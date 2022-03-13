using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Infrastructure.Persistence
{
    public partial class ChangeChannelUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 2,
                column: "Host",
                value: "salonsuisseromand.hbspot.ch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 2,
                column: "Host",
                value: "salonsuisseromand.northeurope.cloudapp.azure.com");
        }
    }
}