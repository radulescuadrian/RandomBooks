using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomBooks.Data.Migrations
{
    public partial class blobAndAuthorModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Blobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ObjectId",
                table: "Blobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_ImageId",
                table: "Authors",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Blobs_ImageId",
                table: "Authors",
                column: "ImageId",
                principalTable: "Blobs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Blobs_ImageId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_ImageId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
