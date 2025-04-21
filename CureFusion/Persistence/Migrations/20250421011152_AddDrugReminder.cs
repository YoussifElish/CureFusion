using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED1aEPNi6peXWyk6PxULmRSSxwb3aHVKSNUUsKxN9EPN7WLZREE1xjmVtHkGvOnADQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP0xqxM2l6hzNs5NRIQnlQnZsDm4MBZjN7j2pT8lvAk7XO+vFu8luGeah8qVmtAGPw==");
        }
    }
}
