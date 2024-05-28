using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagTestId",
                table: "PostTag",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TagTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approved = table.Column<bool>(type: "bit", nullable: true),
                    NumberOfPost = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    catid = table.Column<int>(type: "int", nullable: true)
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
    }
}
