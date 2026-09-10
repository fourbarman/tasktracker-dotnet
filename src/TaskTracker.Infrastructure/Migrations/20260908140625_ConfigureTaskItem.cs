using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "tasks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "tasks",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "tasks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tasks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "tasks",
                newName: "is_completed");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tasks",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "tasks",
                newName: "completed_at");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "tasks",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tasks",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tasks",
                table: "tasks",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_completed_at",
                table: "tasks",
                column: "completed_at");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_created_at",
                table: "tasks",
                column: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tasks",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_completed_at",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_created_at",
                table: "tasks");

            migrationBuilder.RenameTable(
                name: "tasks",
                newName: "Tasks");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Tasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Tasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tasks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_completed",
                table: "Tasks",
                newName: "IsCompleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Tasks",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "completed_at",
                table: "Tasks",
                newName: "CompletedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");
        }
    }
}
