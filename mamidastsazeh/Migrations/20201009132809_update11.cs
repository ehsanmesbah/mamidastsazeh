using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagTestId",
                table: "PostTag",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TagTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: true),
                    NumberOfPost = table.Column<int>(nullable: true),
                    catid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTest", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagTestId",
                table: "PostTag",
                column: "TagTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_TagTest_TagTestId",
                table: "PostTag",
                column: "TagTestId",
                principalTable: "TagTest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_TagTest_TagTestId",
                table: "PostTag");

            migrationBuilder.DropTable(
                name: "TagTest");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_TagTestId",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "TagTestId",
                table: "PostTag");
        }
    }
}
