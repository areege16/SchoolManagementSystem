namespace SchoolManagementSystem.Application.DTOs.Course
{
   public class UpdateCourseDto 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { set; get; }
        public string? Description { set; get; }
        public int? Credits { get; set; }
        public int? DepartmentId { get; set; }
    }
}
