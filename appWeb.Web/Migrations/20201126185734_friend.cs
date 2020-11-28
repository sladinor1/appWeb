using Microsoft.EntityFrameworkCore.Migrations;

namespace appWeb.Web.Migrations
{
    public partial class friend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFullPath",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFullPath",
                table: "Products",
                nullable: true);
        }
    }
}
