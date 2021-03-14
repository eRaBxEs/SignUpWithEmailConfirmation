using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignupWithMailConfirmation.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginInfos",
                columns: table => new
                {
                    UserInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: false),
                    Username = table.Column<string>(maxLength: 16, nullable: false),
                    Password = table.Column<string>(maxLength: 255, nullable: false),
                    IsmailConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginInfos", x => x.UserInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginInfos");
        }
    }
}
