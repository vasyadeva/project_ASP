using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class AddedGroupMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_hubUsers",
                table: "hubUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentifier",
                table: "hubUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "hubUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "GroupChatName",
                table: "hubUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hubUsers",
                table: "hubUsers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_hubUsers_GroupChatName",
                table: "hubUsers",
                column: "GroupChatName");

            migrationBuilder.AddForeignKey(
                name: "FK_hubUsers_hubGroups_GroupChatName",
                table: "hubUsers",
                column: "GroupChatName",
                principalTable: "hubGroups",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hubUsers_hubGroups_GroupChatName",
                table: "hubUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hubUsers",
                table: "hubUsers");

            migrationBuilder.DropIndex(
                name: "IX_hubUsers_GroupChatName",
                table: "hubUsers");

            migrationBuilder.DropColumn(
                name: "id",
                table: "hubUsers");

            migrationBuilder.DropColumn(
                name: "GroupChatName",
                table: "hubUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentifier",
                table: "hubUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hubUsers",
                table: "hubUsers",
                column: "UserIdentifier");
        }
    }
}
