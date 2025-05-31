using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionExpiryMinutes",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                columns: new[] { "PasswordHash", "SessionExpiryMinutes" },
                values: new object[] { "AQAAAAIAAYagAAAAENgIi1jRBlAlrWYMUaUgWxEn41sxzHa3ZSZ9PhgYOikvyv8gpikRtC+GjT+sOevfZA==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionExpiryMinutes",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO7FmD1/dewR/b5CSWhG3Vs6iS1oG5QCnFTMLAyVDv6mjpwRCvrew9TCA/9/btg0EQ==");
        }
    }
}
