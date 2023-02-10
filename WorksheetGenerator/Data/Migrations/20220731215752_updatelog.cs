using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorksheetGenerator.Data.Migrations
{
    public partial class updatelog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Log4NetLog",
                table: "Log4NetLog");

            migrationBuilder.DropColumn(
                name: "Thread",
                table: "Log4NetLog");

            migrationBuilder.RenameTable(
                name: "Log4NetLog",
                newName: "Log");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Log",
                newName: "Logged");

            migrationBuilder.AlterColumn<string>(
                name: "Logger",
                table: "Log",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Application",
                table: "Log",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Callsite",
                table: "Log",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Application",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Callsite",
                table: "Log");

            migrationBuilder.RenameTable(
                name: "Log",
                newName: "Log4NetLog");

            migrationBuilder.RenameColumn(
                name: "Logged",
                table: "Log4NetLog",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "Logger",
                table: "Log4NetLog",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "Thread",
                table: "Log4NetLog",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log4NetLog",
                table: "Log4NetLog",
                column: "Id");
        }
    }
}
