using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class chathub_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrivateChatName",
                table: "HubMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "hubGroups",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hubGroups", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "privateChats",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_privateChats", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "hubGroupMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupChatName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hubGroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hubGroupMessages_hubGroups_GroupChatName",
                        column: x => x.GroupChatName,
                        principalTable: "hubGroups",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubMessages_PrivateChatName",
                table: "HubMessages",
                column: "PrivateChatName");

            migrationBuilder.CreateIndex(
                name: "IX_hubGroupMessages_GroupChatName",
                table: "hubGroupMessages",
                column: "GroupChatName");

            migrationBuilder.AddForeignKey(
                name: "FK_HubMessages_privateChats_PrivateChatName",
                table: "HubMessages",
                column: "PrivateChatName",
                principalTable: "privateChats",
                principalColumn: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubMessages_privateChats_PrivateChatName",
                table: "HubMessages");

            migrationBuilder.DropTable(
                name: "hubGroupMessages");

            migrationBuilder.DropTable(
                name: "privateChats");

            migrationBuilder.DropTable(
                name: "hubGroups");

            migrationBuilder.DropIndex(
                name: "IX_HubMessages_PrivateChatName",
                table: "HubMessages");

            migrationBuilder.DropColumn(
                name: "PrivateChatName",
                table: "HubMessages");
        }
    }
}
