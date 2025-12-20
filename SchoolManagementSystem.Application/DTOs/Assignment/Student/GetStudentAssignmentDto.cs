namespace SchoolManagementSystem.Application.DTOs.Assignment.Student
{
    public class GetStudentAssignmentDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly DueDate { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string CreatedByTeacherName { get; set; }
    }
}