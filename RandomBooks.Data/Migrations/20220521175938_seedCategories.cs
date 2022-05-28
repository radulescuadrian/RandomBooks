using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class seedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Deleted", "Name", "Visible" },
                values: new object[,]
                {
                    { 1, false, "Action and Adventure", true },
                    { 2, false, "Classics", true },
                    { 3, false, "Horror", true },
                    { 4, false, "Fantasy", true },
                    { 5, false, "Romance", true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
