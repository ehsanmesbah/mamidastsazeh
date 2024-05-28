using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "Sms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Sms",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VerificationCodeId",
                table: "Sms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Sms");

            migrationBuilder.DropColumn(
                name: "VerificationCodeId",
                table: "Sms");
        }
    }
}
