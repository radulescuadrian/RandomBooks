using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class cardsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerCards");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerDetailsId",
                table: "CustomerCards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards",
                column: "CustomerDetailsId",
                principalTable: "CustomerDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerDetailsId",
                table: "CustomerCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomerCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards",
                column: "CustomerDetailsId",
                principalTable: "CustomerDetails",
                principalColumn: "Id");
        }
    }
}
