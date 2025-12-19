namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class GradeStudentSubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string StudentId { set; get; }
        public double? Grade { get; set; }
        public string? Remarks { get; set; }
    }
}
