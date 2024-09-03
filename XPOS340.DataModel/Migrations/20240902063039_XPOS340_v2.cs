using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XPOS340.DataModel.Migrations
{
    public partial class XPOS340_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Tbl_Coba",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Tbl_Coba");
        }
    }
}
