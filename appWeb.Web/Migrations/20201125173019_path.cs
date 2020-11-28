using Microsoft.EntityFrameworkCore.Migrations;

namespace appWeb.Web.Migrations
{
    public partial class path : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFullPath",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFullPath",
                table: "Products");
        }
    }
}
