using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class customerDetailsRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCards_Users_UserId",
                table: "CustomerCards");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCards_UserId",
                table: "CustomerCards");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "CustomerDetailsId",
                table: "CustomerCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerDetailsId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCards_CustomerDetailsId",
                table: "CustomerCards",
                column: "CustomerDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerDetailsId",
                table: "Addresses",
                column: "CustomerDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CustomerDetails_CustomerDetailsId",
                table: "Addresses",
                column: "CustomerDetailsId",
                principalTable: "CustomerDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards",
                column: "CustomerDetailsId",
                principalTable: "CustomerDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CustomerDetails_CustomerDetailsId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCards_CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerDetailsId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.DropColumn(
                name: "CustomerDetailsId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCards_UserId",
                table: "CustomerCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCards_Users_UserId",
                table: "CustomerCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
