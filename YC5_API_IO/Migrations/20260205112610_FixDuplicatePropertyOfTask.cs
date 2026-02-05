using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YC5_API_IO.Migrations
{
    /// <inheritdoc />
    public partial class FixDuplicatePropertyOfTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "TaskPriority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskPriority",
                table: "Tasks",
                newName: "Status");
        }
    }
}
