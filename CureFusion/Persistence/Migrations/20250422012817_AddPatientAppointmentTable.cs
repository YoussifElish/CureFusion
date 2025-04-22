using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DoctorAvailabilities");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Appointments");

            migrationBuilder.CreateTable(
                name: "patientAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patientAppointments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientAppointments_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMcKhGNdHfogiCC4CuYeLmAnhcnb1r6H9HN0+KFlc5Ht3HE+fpQRCm2E04CGWRrLgw==");

            migrationBuilder.CreateIndex(
                name: "IX_patientAppointments_AppointmentId",
                table: "patientAppointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_patientAppointments_UserId1",
                table: "patientAppointments",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patientAppointments");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DoctorAvailabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHyApa1AR+lXsz6C2sTym+gnXntMofFwpxk+Y4tEY3BB/vHrlvRlzaRJZMA5i1M5Rw==");
        }
    }
}
