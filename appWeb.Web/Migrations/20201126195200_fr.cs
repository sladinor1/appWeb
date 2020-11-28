using Microsoft.EntityFrameworkCore.Migrations;

namespace appWeb.Web.Migrations
{
    public partial class fr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SecondPersonId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "FirstPersonId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SecondPersonId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "FirstPersonId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
