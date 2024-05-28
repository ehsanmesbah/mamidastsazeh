using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "instapostid",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "istitlecorrect",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instapostid",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "istitlecorrect",
                table: "Post");
        }
    }
}
