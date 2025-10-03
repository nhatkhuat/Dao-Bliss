using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoBlissWebApp.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Posts_V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_AuthorId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Articles",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                newName: "IX_Articles_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_ApplicationUserId",
                table: "Articles",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_ApplicationUserId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Articles",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_ApplicationUserId",
                table: "Articles",
                newName: "IX_Articles_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_AuthorId",
                table: "Articles",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
