using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugReminderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SideEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<int>(type: "int", nullable: false),
                    Interaction = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugReminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReminderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepeatIntervalMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugReminders_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrugReminders_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFWxiPyrkVHM2OnqLVaRcgF6v0xVzw+vxhO1dHWLn18TGQe6qjS8pCxDXdI0syfB1Q==");

            migrationBuilder.CreateIndex(
                name: "IX_DrugReminders_DrugId",
                table: "DrugReminders",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugReminders_UserId1",
                table: "DrugReminders",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugReminders");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED1aEPNi6peXWyk6PxULmRSSxwb3aHVKSNUUsKxN9EPN7WLZREE1xjmVtHkGvOnADQ==");
        }
    }
}
