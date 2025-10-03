using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoBlissWebApp.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Post_V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "FeaturedImageUrl",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImageUrl",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Articles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Articles",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
