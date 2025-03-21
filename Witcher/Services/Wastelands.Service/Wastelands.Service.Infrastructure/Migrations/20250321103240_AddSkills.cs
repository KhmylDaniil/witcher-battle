using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Wastelands.Service.Domain.Enums;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<Skill, int>>(
                name: "Skills",
                table: "Character",
                type: "jsonb",
                nullable: false,
                comment: "Skills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Character");
        }
    }
}
