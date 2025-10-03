using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoBlissWebApp.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Feedback_V8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductReviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "ProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
