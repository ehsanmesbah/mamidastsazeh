using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendType",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendType",
                table: "Post");
        }
    }
}
