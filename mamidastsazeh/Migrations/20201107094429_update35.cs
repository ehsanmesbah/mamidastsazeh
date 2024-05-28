using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Post");

            migrationBuilder.AddColumn<long>(
                name: "ParentCommentId",
                table: "PostComment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_ParentCommentId",
                table: "PostComment",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_PostComment_ParentCommentId",
                table: "PostComment",
                column: "ParentCommentId",
                principalTable: "PostComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_PostComment_ParentCommentId",
                table: "PostComment");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_ParentCommentId",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "PostComment");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Post",
                type: "bit",
                nullable: true);
        }
    }
}
