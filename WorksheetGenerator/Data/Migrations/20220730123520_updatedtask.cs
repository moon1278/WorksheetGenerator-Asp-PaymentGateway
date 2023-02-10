using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorksheetGenerator.Data.Migrations
{
    public partial class updatedtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "Task",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "Task");
        }
    }
}
