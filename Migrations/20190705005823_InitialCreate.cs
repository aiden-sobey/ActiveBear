using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActiveBear.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    User = table.Column<Guid>(nullable: false),
                    Channel = table.Column<Guid>(nullable: false),
                    HashedKey = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelAuths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    KeyHash = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sender = table.Column<string>(nullable: false),
                    Channel = table.Column<Guid>(nullable: false),
                    EncryptedContents = table.Column<string>(nullable: false),
                    SendDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    CookieId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelAuths");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
