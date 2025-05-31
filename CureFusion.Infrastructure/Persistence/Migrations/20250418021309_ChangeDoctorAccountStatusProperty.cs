using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDoctorAccountStatusProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "accountStatus",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountStatus",
                table: "Doctors");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
