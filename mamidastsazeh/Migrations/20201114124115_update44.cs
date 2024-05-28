using Microsoft.EntityFrameworkCore.Migrations;

namespace mamidastsazeh.Migrations
{
    public partial class update44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MamiEvent",
                table: "MamiEvent");

            migrationBuilder.DropColumn(
                name: "Id_new",
                table: "MamiEvent");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "MamiEvent",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MamiEvent",
                table: "MamiEvent",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MamiEvent",
                table: "MamiEvent");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MamiEvent");

            migrationBuilder.AddColumn<long>(
                name: "Id_new",
                table: "MamiEvent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MamiEvent",
                table: "MamiEvent",
                column: "Id_new");
        }
    }
}
