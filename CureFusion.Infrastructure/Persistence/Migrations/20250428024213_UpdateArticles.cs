using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CureFusion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishedIn",
                table: "HealthArticles",
                newName: "PublishedDate");

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "HealthArticles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "HealthArticles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "HealthArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "HealthArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "HealthArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "HealthArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationCodeExpiration",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordCodeExpiration",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                columns: new[] { "EmailConfirmationCode", "EmailConfirmationCodeExpiration", "PasswordHash", "ResetPasswordCode", "ResetPasswordCodeExpiration" },
                values: new object[] { null, null, "AQAAAAIAAYagAAAAEP55VrINkT0ScgCDWRuOTDik6A9acRx1EdGHUv0Rpe794D2Ud589bhySAngS4XMVMg==", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_HealthArticles_AuthorId",
                table: "HealthArticles",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthArticles_AspNetUsers_AuthorId",
                table: "HealthArticles",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthArticles_AspNetUsers_AuthorId",
                table: "HealthArticles");

            migrationBuilder.DropIndex(
                name: "IX_HealthArticles_AuthorId",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "HealthArticles");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationCodeExpiration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordCodeExpiration",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PublishedDate",
                table: "HealthArticles",
                newName: "PublishedIn");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "HealthArticles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7292e4ca-33b6-4b21-a314-7f72262391fd",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHH/xdCKjtZUDEuwA2JGaUZFBHZRBYsgXcRV+w+sIg7osG6zogqMdhpBtKhTLsxbDA==");
        }
    }
}
