using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIndexByCreatedAtCreatedIntexByIsCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_completed_at",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_is_completed",
                table: "tasks",
                column: "is_completed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_is_completed",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_completed_at",
                table: "tasks",
                column: "completed_at");
        }
    }
}
