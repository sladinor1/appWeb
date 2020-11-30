using Microsoft.EntityFrameworkCore.Migrations;

namespace appWeb.Web.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Relation",
                table: "Friends",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Relation",
                table: "Friends");
        }
    }
}
