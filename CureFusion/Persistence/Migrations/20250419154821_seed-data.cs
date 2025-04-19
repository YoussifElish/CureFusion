using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46235578-03e2-43cf-9344-6a0c7b20925d", "d7aaa1d4-a150-4044-b84c-2c59417140f7", true, false, "Member", "MEMBER" },
                    { "aa6f1471-6662-4a6e-97b9-d27745585bda", "8c841d4d-5a09-42a9-b6ff-9d7f362f5477", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "Appoitments:read", "aa6f1471-6662-4a6e-97b9-d27745585bda" },
                    { 2, "permissions", "Appoitments:add", "aa6f1471-6662-4a6e-97b9-d27745585bda" },
                    { 3, "permissions", "Appoitments:update", "aa6f1471-6662-4a6e-97b9-d27745585bda" },
                    { 4, "permissions", "Appoitments:delete", "aa6f1471-6662-4a6e-97b9-d27745585bda" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46235578-03e2-43cf-9344-6a0c7b20925d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa6f1471-6662-4a6e-97b9-d27745585bda");
        }
    }
}
