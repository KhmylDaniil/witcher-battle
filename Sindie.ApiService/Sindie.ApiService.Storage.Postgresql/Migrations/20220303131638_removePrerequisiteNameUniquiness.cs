using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class removePrerequisiteNameUniquiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prerequisites_Name",
                schema: "GameRules",
                table: "Prerequisites");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_Name",
                schema: "GameRules",
                table: "Prerequisites",
                column: "Name",
                unique: true);
        }
    }
}
