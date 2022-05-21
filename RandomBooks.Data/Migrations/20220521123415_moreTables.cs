using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class moreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Card_Users_UserId",
                table: "User_Card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Card",
                table: "User_Card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "User_Card",
                newName: "UserCards");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_User_Card_UserId",
                table: "UserCards",
                newName: "IX_UserCards_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserId",
                table: "Addresses",
                newName: "IX_Addresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCards_Users_UserId",
                table: "UserCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCards_Users_UserId",
                table: "UserCards");

            migrationBuilder.DropTable(
                name: "Blobs");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "UserCards",
                newName: "User_Card");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_UserCards_UserId",
                table: "User_Card",
                newName: "IX_User_Card_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserId",
                table: "Address",
                newName: "IX_Address_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Card",
                table: "User_Card",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Card_Users_UserId",
                table: "User_Card",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
