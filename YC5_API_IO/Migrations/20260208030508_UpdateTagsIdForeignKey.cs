using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YC5_API_IO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTagsIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Tasks_TaskId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Tasks_TaskId",
                table: "Reminders",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Tasks_TaskId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Tasks_TaskId",
                table: "Reminders",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
