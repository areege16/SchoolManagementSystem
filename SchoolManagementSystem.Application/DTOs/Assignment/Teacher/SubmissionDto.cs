namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class SubmissionDto
    {
        public string StudentId { get; set; }
        public string? StudentName { get; set; } 
        public DateTime SubmittedDate { get; set; }
        public string? OriginalFileName { get; set; }
        public double? Grade { get; set; }
        public string? Remarks { get; set; }
    }
}
