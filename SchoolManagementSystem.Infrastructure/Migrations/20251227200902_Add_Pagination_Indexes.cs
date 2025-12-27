using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Pagination_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId_EnrollmentDate",
                table: "StudentClasses",
                columns: new[] { "StudentId", "EnrollmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId_Id",
                table: "Classes",
                columns: new[] { "TeacherId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentClasses_StudentId_EnrollmentDate",
                table: "StudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherId_Id",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");
        }
    }
}
