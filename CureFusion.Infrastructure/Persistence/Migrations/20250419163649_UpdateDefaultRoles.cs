using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "RefreshTokens",
                newName: "UserId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46235578-03e2-43cf-9344-6a0c7b20925d",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Patient", "PATIENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[] { "46235578-03e2-49bg-9344-6a0c7b20925d", "d7aaa1d4-s256-4044-b84c-2c59417140f7", false, false, "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DOB", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7292e4ca-33b6-4b21-a314-7f72262391fd", 0, "334dcfc8-a525-4a0b-b92f-62c571f9c439", new DateOnly(1, 1, 1), "admin@CureFusion.com", true, "CureFusion", false, "Admin", false, null, "ADMIN@CUREFUSION.COM", "ADMIN@CUREFUSION.COM", "AQAAAAIAAYagAAAAEEttQECAA1F/ja/6yXbctYZK/Hbk/zQVFY/jQysHXQM+gQ35xNmCvrIqC3AaDWbjKw==", null, false, "07DB2EDBB86447CA8B2EC4E293AE89F5", false, "admin@CureFusion.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "aa6f1471-6662-4a6e-97b9-d27745585bda", "7292e4ca-33b6-4b21-a314-7f72262391fd" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46235578-03e2-49bg-9344-6a0c7b20925d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "aa6f1471-6662-4a6e-97b9-d27745585bda", "7292e4ca-33b6-4b21-a314-7f72262391fd" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshTokens",
                newName: "ApplicationUserId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46235578-03e2-43cf-9344-6a0c7b20925d",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Member", "MEMBER" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
