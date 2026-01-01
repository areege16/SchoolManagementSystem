using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.DTOs.Course
{
    public class ClassMiniDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Semester Semester { set; get; }
        public DateOnly StartDate { set; get; }
        public DateOnly EndDate { set; get; }
    }
}