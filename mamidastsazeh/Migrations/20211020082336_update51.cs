using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPrice",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendTypeId",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SendType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_SendTypeId",
                table: "Post",
                column: "SendTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_SendType_SendTypeId",
                table: "Post",
                column: "SendTypeId",
                principalTable: "SendType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_SendType_SendTypeId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "SendType");

            migrationBuilder.DropIndex(
                name: "IX_Post_SendTypeId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "SendTypeId",
                table: "Post");
        }
    }
}
