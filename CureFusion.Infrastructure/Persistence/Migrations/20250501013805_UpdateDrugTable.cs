using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDrugTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dosage",
                table: "Drugs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIeypGwaduUGH7BLqXRpja1NI++2gcsk6dkSsh+0P3tfw58OqNBOMfVFPHk/F+QP+Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Dosage",
                table: "Drugs",
                type: "int",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJPvF7IrSKGHsAEOwKGw3Gg8tPdSzrFYVljrkvV4l2+xPpC5ZZFEZOJkWhOWNJB0Xg==");
        }
    }
}
