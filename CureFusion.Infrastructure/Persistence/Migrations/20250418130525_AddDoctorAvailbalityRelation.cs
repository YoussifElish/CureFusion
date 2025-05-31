using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAvailbalityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorAvailabilityId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorAvailabilityId",
                table: "Appointments",
                column: "DoctorAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_DoctorAvailabilityId",
                table: "Appointments",
                column: "DoctorAvailabilityId",
                principalTable: "DoctorAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_DoctorAvailabilityId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorAvailabilityId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DoctorAvailabilityId",
                table: "Appointments");
        }
    }
}
