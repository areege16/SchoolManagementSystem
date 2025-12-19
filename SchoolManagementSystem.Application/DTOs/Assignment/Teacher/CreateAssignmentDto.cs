namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class CreateAssignmentDto
    {
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateTime DueDate { set; get; }
        public int ClassId { set; get; }
    }
}