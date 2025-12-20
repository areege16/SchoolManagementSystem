namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class AssignmentDto
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateOnly DueDate { set; get; }
        public int ClassId { set; get; }
        public string CreatedByTeacherId { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
