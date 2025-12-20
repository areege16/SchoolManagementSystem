namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class GetTeacherAssignmentDto
    {
        public int Id { get; set; }
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateOnly DueDate { set; get; }
        public int ClassId { set; get; }
        public DateTime CreatedDate { get; set; }
    }
}