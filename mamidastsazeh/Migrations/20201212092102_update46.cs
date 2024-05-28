using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FalseReporting",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "FalseReporting",
                table: "AspNetUsers");
        }
    }
}
