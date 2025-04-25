using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePatientAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientAppointments_AspNetUsers_UserId1",
                table: "patientAppointments");

            migrationBuilder.DropIndex(
                name: "IX_patientAppointments_UserId1",
                table: "patientAppointments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "patientAppointments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "patientAppointments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PaymentUrl",
                table: "patientAppointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerSlot",
                table: "Appointments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOO3G0kAbVimAav8fPnGZJFEZz9chp1yIA5Ynma/ubocmzYU61f/VfndfyVQVDBTQA==");

            migrationBuilder.CreateIndex(
                name: "IX_patientAppointments_UserId",
                table: "patientAppointments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientAppointments_AspNetUsers_UserId",
                table: "patientAppointments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientAppointments_AspNetUsers_UserId",
                table: "patientAppointments");

            migrationBuilder.DropIndex(
                name: "IX_patientAppointments_UserId",
                table: "patientAppointments");

            migrationBuilder.DropColumn(
                name: "PaymentUrl",
                table: "patientAppointments");

            migrationBuilder.DropColumn(
                name: "PricePerSlot",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "patientAppointments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "patientAppointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMcKhGNdHfogiCC4CuYeLmAnhcnb1r6H9HN0+KFlc5Ht3HE+fpQRCm2E04CGWRrLgw==");

            migrationBuilder.CreateIndex(
                name: "IX_patientAppointments_UserId1",
                table: "patientAppointments",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_patientAppointments_AspNetUsers_UserId1",
                table: "patientAppointments",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
