using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeauditableFromAppointmentAndAddNotesProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedByID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_UpdatedById",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CreatedByID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_UpdatedById",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DoctorAvailabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DoctorAvailabilities");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByID",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedByID",
                table: "Appointments",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UpdatedById",
                table: "Appointments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedByID",
                table: "Appointments",
                column: "CreatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_UpdatedById",
                table: "Appointments",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
