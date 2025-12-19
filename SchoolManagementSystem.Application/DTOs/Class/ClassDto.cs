using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.DTOs.Class
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { set; get; } = true;
        public Semester Semester { set; get; }
        public DateOnly StartDate { set; get; }
        public DateOnly EndDate { set; get; }
        public int CourseId { set; get; }
        public string? TeacherId { get; set; }
        public List<StudentsInClassDto>? StudentsInClass { get; set; } = new List<StudentsInClassDto>();
    }
}
