using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YC5_API_IO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTagIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
