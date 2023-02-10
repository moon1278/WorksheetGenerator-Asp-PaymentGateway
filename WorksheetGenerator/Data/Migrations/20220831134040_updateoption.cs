using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorksheetGenerator.Data.Migrations
{
    public partial class updateoption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Specification_SpecificationId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_RL_WorksheetClass_Class_ClassId",
                table: "RL_WorksheetClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RL_WorksheetClass_Worksheet_WorksheetId",
                table: "RL_WorksheetClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RL_WorksheetTask_Task_TaskId",
                table: "RL_WorksheetTask");

            migrationBuilder.DropForeignKey(
                name: "FK_RL_WorksheetTask_Worksheet_WorksheetId",
                table: "RL_WorksheetTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Specification_Task_TaskId",
                table: "Specification");

            migrationBuilder.DropIndex(
                name: "IX_Specification_TaskId",
                table: "Specification");

            migrationBuilder.DropIndex(
                name: "IX_RL_WorksheetTask_TaskId",
                table: "RL_WorksheetTask");

            migrationBuilder.DropIndex(
                name: "IX_RL_WorksheetTask_WorksheetId",
                table: "RL_WorksheetTask");

            migrationBuilder.DropIndex(
                name: "IX_RL_WorksheetClass_ClassId",
                table: "RL_WorksheetClass");

            migrationBuilder.DropIndex(
                name: "IX_RL_WorksheetClass_WorksheetId",
                table: "RL_WorksheetClass");

            migrationBuilder.DropIndex(
                name: "IX_Option_SpecificationId",
                table: "Option");

            migrationBuilder.AlterColumn<string>(
                name: "Dynamic_Replace_Text",
                table: "Specification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dynamic_Replace_Text",
                table: "Specification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specification_TaskId",
                table: "Specification",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_WorksheetTask_TaskId",
                table: "RL_WorksheetTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_WorksheetTask_WorksheetId",
                table: "RL_WorksheetTask",
                column: "WorksheetId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_WorksheetClass_ClassId",
                table: "RL_WorksheetClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_WorksheetClass_WorksheetId",
                table: "RL_WorksheetClass",
                column: "WorksheetId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_SpecificationId",
                table: "Option",
                column: "SpecificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Specification_SpecificationId",
                table: "Option",
                column: "SpecificationId",
                principalTable: "Specification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RL_WorksheetClass_Class_ClassId",
                table: "RL_WorksheetClass",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RL_WorksheetClass_Worksheet_WorksheetId",
                table: "RL_WorksheetClass",
                column: "WorksheetId",
                principalTable: "Worksheet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RL_WorksheetTask_Task_TaskId",
                table: "RL_WorksheetTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RL_WorksheetTask_Worksheet_WorksheetId",
                table: "RL_WorksheetTask",
                column: "WorksheetId",
                principalTable: "Worksheet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specification_Task_TaskId",
                table: "Specification",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
