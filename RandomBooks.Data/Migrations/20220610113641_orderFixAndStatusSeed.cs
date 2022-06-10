using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class orderFixAndStatusSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Placed" },
                    { 2, "Being Prepared" },
                    { 3, "Ready to Deliver" },
                    { 4, "Delivery in Progress" },
                    { 5, "Delivered" }
                });

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
                name: "FK_CustomerCards_CustomerDetails_CustomerDetailsId",
                table: "CustomerCards");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5);

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
    }
}
