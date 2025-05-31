using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionanswercolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENxHvMJ6suJKqj0+/wP/0ZWkr6b67A2koIJa+9oOq2idELij7KobfvR+El54DHvq9w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPCPiN5pEsWlqkIXanBoA3u/C6Hff/JwznPah9i8cypRijmz/iedtGZtIyodDMomZQ==");
        }
    }
}
