using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameImageUrlToImageKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "CreatureTemplate",
                newName: "ImageKey");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Character",
                newName: "ImageKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageKey",
                table: "CreatureTemplate",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageKey",
                table: "Character",
                newName: "ImageUrl");
        }
    }
}
