using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadedFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificationDocumentUrl",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentUrl",
                table: "patientAppointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthArticleImageId",
                table: "HealthArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drugs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DrugImageId",
                table: "Drugs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CertificationDocumentId",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                columns: new[] { "PasswordHash", "ProfileImageId" },
                values: new object[] { "AQAAAAIAAYagAAAAEHH/xdCKjtZUDEuwA2JGaUZFBHZRBYsgXcRV+w+sIg7osG6zogqMdhpBtKhTLsxbDA==", null });

            migrationBuilder.CreateIndex(
                name: "IX_HealthArticles_HealthArticleImageId",
                table: "HealthArticles",
                column: "HealthArticleImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_DrugImageId",
                table: "Drugs",
                column: "DrugImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CertificationDocumentId",
                table: "Doctors",
                column: "CertificationDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ProfileImageId",
                table: "Doctors",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileImageId",
                table: "AspNetUsers",
                column: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UploadedFiles_ProfileImageId",
                table: "AspNetUsers",
                column: "ProfileImageId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_UploadedFiles_CertificationDocumentId",
                table: "Doctors",
                column: "CertificationDocumentId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_UploadedFiles_ProfileImageId",
                table: "Doctors",
                column: "ProfileImageId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_UploadedFiles_DrugImageId",
                table: "Drugs",
                column: "DrugImageId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthArticles_UploadedFiles_HealthArticleImageId",
                table: "HealthArticles",
                column: "HealthArticleImageId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UploadedFiles_ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_UploadedFiles_CertificationDocumentId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_UploadedFiles_ProfileImageId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_UploadedFiles_DrugImageId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthArticles_UploadedFiles_HealthArticleImageId",
                table: "HealthArticles");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropIndex(
                name: "IX_HealthArticles_HealthArticleImageId",
                table: "HealthArticles");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_DrugImageId",
                table: "Drugs");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_CertificationDocumentId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ProfileImageId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HealthArticleImageId",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "DrugImageId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "CertificationDocumentId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentUrl",
                table: "patientAppointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "CertificationDocumentUrl",
                table: "Doctors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOO3G0kAbVimAav8fPnGZJFEZz9chp1yIA5Ynma/ubocmzYU61f/VfndfyVQVDBTQA==");
        }
    }
}
