namespace SchoolManagementSystem.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? HeadOfDepartmentId { get; set; }
    }
}