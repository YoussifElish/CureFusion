using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDrugReminderUserTypeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugReminders_AspNetUsers_UserId1",
                table: "DrugReminders");

            migrationBuilder.DropIndex(
                name: "IX_DrugReminders_UserId1",
                table: "DrugReminders");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "DrugReminders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DrugReminders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHyApa1AR+lXsz6C2sTym+gnXntMofFwpxk+Y4tEY3BB/vHrlvRlzaRJZMA5i1M5Rw==");

            migrationBuilder.CreateIndex(
                name: "IX_DrugReminders_UserId",
                table: "DrugReminders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugReminders_AspNetUsers_UserId",
                table: "DrugReminders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugReminders_AspNetUsers_UserId",
                table: "DrugReminders");

            migrationBuilder.DropIndex(
                name: "IX_DrugReminders_UserId",
                table: "DrugReminders");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "DrugReminders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "DrugReminders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFWxiPyrkVHM2OnqLVaRcgF6v0xVzw+vxhO1dHWLn18TGQe6qjS8pCxDXdI0syfB1Q==");

            migrationBuilder.CreateIndex(
                name: "IX_DrugReminders_UserId1",
                table: "DrugReminders",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugReminders_AspNetUsers_UserId1",
                table: "DrugReminders",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
