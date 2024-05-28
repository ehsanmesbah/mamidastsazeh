using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sms_Post_PostID",
                table: "Sms");

            migrationBuilder.DropIndex(
                name: "IX_Sms_PostID",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "PostID",
                table: "Sms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostID",
                table: "Sms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sms_PostID",
                table: "Sms",
                column: "PostID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sms_Post_PostID",
                table: "Sms",
                column: "PostID",
                principalTable: "Post",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
