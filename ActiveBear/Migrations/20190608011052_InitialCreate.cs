using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActiveBear.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sender = table.Column<Guid>(nullable: false),
                    Channel = table.Column<Guid>(nullable: false),
                    EncryptedContents = table.Column<string>(nullable: true),
                    SendDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    ChannelId = table.Column<Guid>(nullable: true),
                    UserEncryptedKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelAuths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    CreateUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Users_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelAuths_ChannelId",
                table: "ChannelAuths",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelAuths_UserId",
                table: "ChannelAuths",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CreateUserId",
                table: "Channels",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChannelId",
                table: "Users",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAuths_Channels_ChannelId",
                table: "ChannelAuths",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAuths_Users_UserId",
                table: "ChannelAuths",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Channels_ChannelId",
                table: "Users",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Channels_ChannelId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ChannelAuths");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
