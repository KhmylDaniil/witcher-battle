using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecoveryStunAndCurrentHp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Recovery",
                table: "Creature",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "(Body+Will шаблона)/2, вычисляется на сервере");

            migrationBuilder.AddColumn<int>(
                name: "Stun",
                table: "Creature",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "(Body+Will шаблона)/2, вычисляется на сервере");

            migrationBuilder.AddColumn<int>(
                name: "CurrentHP",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Recovery",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "(Str+Wil)/2, вычисляется на сервере");

            migrationBuilder.AddColumn<int>(
                name: "Stun",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "(Str+Wil)/2, вычисляется на сервере");

            // Бэкафилл для строк, созданных до появления этих колонок — иначе они остались бы с
            // CurrentHP/Recovery/Stun = 0 (мёртвый персонаж, нулевой отдых) до следующего редактирования.
            migrationBuilder.Sql(
                """
                UPDATE "Character" SET "CurrentHP" = "HP", "Recovery" = ("Str" + "Wil") / 2, "Stun" = ("Str" + "Wil") / 2;
                """);
            migrationBuilder.Sql(
                """
                UPDATE "Creature" c
                SET "Recovery" = (ct."Body" + ct."Will") / 2, "Stun" = (ct."Body" + ct."Will") / 2
                FROM "CreatureTemplate" ct
                WHERE c."CreatureTemplateId" = ct."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recovery",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "Stun",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "CurrentHP",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Recovery",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Stun",
                table: "Character");
        }
    }
}
