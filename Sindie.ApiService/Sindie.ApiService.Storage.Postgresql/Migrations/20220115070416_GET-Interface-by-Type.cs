using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class GETInterfacebyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "SystemDarkTheme", "SystemInterface" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"),
                columns: new[] { "Name", "Type" },
                values: new object[] { " SystemLightTheme", "SystemInterface" });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Interfaces",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "Type" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GameDarkTheme", "Default", "Default", "GameInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d9"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GameLightTheme", "Default", "Default", "GameInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d5"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CharacterDarkTheme", "Default", "Default", "CharacterInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d0"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CharacterLightTheme", "Default", "Default", "CharacterInterface" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d0"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d5"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d9"));

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "DarkTheme", "DarkThemeType" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "LightTheme", "LightThemeType" });
        }
    }
}
