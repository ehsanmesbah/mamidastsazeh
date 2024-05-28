using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostID",
                table: "Sms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sms_PostID",
                table: "Sms",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Sms_UserId",
                table: "Sms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sms_Post_PostID",
                table: "Sms",
                column: "PostID",
                principalTable: "Post",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sms_AspNetUsers_UserId",
                table: "Sms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sms_Post_PostID",
                table: "Sms");

            migrationBuilder.DropForeignKey(
                name: "FK_Sms_AspNetUsers_UserId",
                table: "Sms");

            migrationBuilder.DropIndex(
                name: "IX_Sms_PostID",
                table: "Sms");

            migrationBuilder.DropIndex(
                name: "IX_Sms_UserId",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "PostID",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sms");
        }
    }
}
