﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class settingdefaultvaluetoquestionuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPCPiN5pEsWlqkIXanBoA3u/C6Hff/JwznPah9i8cypRijmz/iedtGZtIyodDMomZQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBFFv/F7RAkW2FfLroXOffAGzgNPdHJFGzhkGdj3Z6AYr7eYEAEivSzbw2mJzUKkeQ==");
        }
    }
}
