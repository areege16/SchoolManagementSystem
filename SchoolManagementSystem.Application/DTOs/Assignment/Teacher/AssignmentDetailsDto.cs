namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class AssignmentDetailsDto
    {
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateOnly DueDate { set; get; }
        public DateTime CreatedDate { get; set; }
        public List<SubmissionDto> Submissions { get; set; } = new List<SubmissionDto>();
    }
}