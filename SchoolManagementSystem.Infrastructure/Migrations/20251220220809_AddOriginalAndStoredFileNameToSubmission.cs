using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginalAndStoredFileNameToSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Submissions",
                newName: "StoredFileName");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "StoredFileName",
                table: "Submissions",
                newName: "FileUrl");
        }
    }
}
