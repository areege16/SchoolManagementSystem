namespace SchoolManagementSystem.Application.DTOs.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? HeadOfDepartmentId { get; set; }
        public List<CourseMiniDto>? Courses { get; set; }

    }
}