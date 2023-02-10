using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorksheetGenerator.Data.Migrations
{
    public partial class tokendbupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Token");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Token",
                newName: "PayAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Token",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "CreditAmount",
                table: "Token",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Token",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TokenViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditAmount = table.Column<float>(type: "real", nullable: false),
                    PayAmount = table.Column<float>(type: "real", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenViewModel_TokenViewModel_TokenViewModelId",
                        column: x => x.TokenViewModelId,
                        principalTable: "TokenViewModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenViewModel_TokenViewModelId",
                table: "TokenViewModel",
                column: "TokenViewModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenViewModel");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Token");

            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "Token");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Token");

            migrationBuilder.RenameColumn(
                name: "PayAmount",
                table: "Token",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Token",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
