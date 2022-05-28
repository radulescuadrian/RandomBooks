using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class modifiedCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVC",
                table: "UserCards");

            migrationBuilder.AddColumn<string>(
                name: "Last4Numbers",
                table: "UserCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Last4Numbers",
                table: "UserCards");

            migrationBuilder.AddColumn<int>(
                name: "CVC",
                table: "UserCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
