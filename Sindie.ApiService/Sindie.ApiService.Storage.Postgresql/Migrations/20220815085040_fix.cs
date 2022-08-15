using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BodyPartLocation",
                schema: "Battles",
                table: "CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тип части тела");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyPartLocation",
                schema: "Battles",
                table: "CritEffects");
        }
    }
}
