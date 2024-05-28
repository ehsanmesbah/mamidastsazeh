using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Tag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPost",
                table: "Tag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "catid",
                table: "Tag",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "NumberOfPost",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "catid",
                table: "Tag");
        }
    }
}
