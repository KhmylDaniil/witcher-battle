using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBodyAndCreatureTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    BodyTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    CreatureType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    HP = table.Column<int>(type: "integer", nullable: false),
                    Sta = table.Column<int>(type: "integer", nullable: false),
                    Int = table.Column<int>(type: "integer", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    Dex = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<int>(type: "integer", nullable: false),
                    Emp = table.Column<int>(type: "integer", nullable: false),
                    Cra = table.Column<int>(type: "integer", nullable: false),
                    Will = table.Column<int>(type: "integer", nullable: false),
                    Speed = table.Column<int>(type: "integer", nullable: false),
                    Luck = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BodyTemplatePart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BodyTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    BodyPartType = table.Column<int>(type: "integer", nullable: false),
                    DamageModifier = table.Column<double>(type: "double precision", nullable: false),
                    HitPenalty = table.Column<int>(type: "integer", nullable: false),
                    MinToHit = table.Column<int>(type: "integer", nullable: false),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplatePart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyTemplatePart_BodyTemplate_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalTable: "BodyTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplatePart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatureTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    BodyPartType = table.Column<int>(type: "integer", nullable: false),
                    DamageModifier = table.Column<double>(type: "double precision", nullable: false),
                    HitPenalty = table.Column<int>(type: "integer", nullable: false),
                    MinToHit = table.Column<int>(type: "integer", nullable: false),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false),
                    Armor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplatePart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplatePart_CreatureTemplate_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalTable: "CreatureTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplatePart_BodyTemplateId",
                table: "BodyTemplatePart",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplatePart_CreatureTemplateId",
                table: "CreatureTemplatePart",
                column: "CreatureTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyTemplatePart");

            migrationBuilder.DropTable(
                name: "CreatureTemplatePart");

            migrationBuilder.DropTable(
                name: "BodyTemplate");

            migrationBuilder.DropTable(
                name: "CreatureTemplate");
        }
    }
}
