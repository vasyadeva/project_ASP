using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedNewsChatApp.Migrations
{
    public partial class NewsCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "news",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "news");
        }
    }
}
