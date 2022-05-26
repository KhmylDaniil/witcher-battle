using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AddGameRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "System",
                table: "GameRoles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MainMaster", "Default", "Default" },
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe117d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Master", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe137d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Player", "Default", "Default" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "System",
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3147-4791-9053-9667cbe117d7"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "GameRoles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3148-4791-9053-9667cbe137d8"));
        }
    }
}
