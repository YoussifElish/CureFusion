using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_description_to_drug_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Drugs",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO7FmD1/dewR/b5CSWhG3Vs6iS1oG5QCnFTMLAyVDv6mjpwRCvrew9TCA/9/btg0EQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Drugs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIeypGwaduUGH7BLqXRpja1NI++2gcsk6dkSsh+0P3tfw58OqNBOMfVFPHk/F+QP+Q==");
        }
    }
}
