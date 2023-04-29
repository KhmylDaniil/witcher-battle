using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class notificationsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isReaded",
                schema: "Notifications",
                table: "Notifications",
                newName: "IsReaded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsReaded",
                schema: "Notifications",
                table: "Notifications",
                newName: "isReaded");
        }
    }
}
