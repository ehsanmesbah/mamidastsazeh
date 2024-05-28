using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Sms");

            migrationBuilder.AddColumn<string>(
                name: "ServerMessage",
                table: "Sms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerMessage",
                table: "Sms");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Sms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
