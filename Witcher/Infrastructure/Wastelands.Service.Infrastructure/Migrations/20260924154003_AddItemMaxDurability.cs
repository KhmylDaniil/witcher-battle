using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItemMaxDurability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxDurability",
                table: "Item",
                type: "integer",
                nullable: true);

            // Бэкфилл для уже существующих экземпляров оружия — MaxDurability не хранился раньше, поэтому
            // берём текущее значение прочности шаблона (лучшее доступное приближение к исходному
            // максимуму на момент добавления в инвентарь) и подстраховываемся уже сохранённым значением
            // прочности самого предмета на случай, если шаблон с тех пор изменился или удалён.
            migrationBuilder.Sql(
                """
                UPDATE "Item" i
                SET "MaxDurability" = GREATEST(i."Durability", COALESCE(t."Durability", i."Durability"))
                FROM "ItemTemplate" t
                WHERE i."ItemTemplateId" = t."Id" AND i."ItemType" = 0;

                UPDATE "Item"
                SET "MaxDurability" = "Durability"
                WHERE "ItemType" = 0 AND "MaxDurability" IS NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDurability",
                table: "Item");
        }
    }
}
