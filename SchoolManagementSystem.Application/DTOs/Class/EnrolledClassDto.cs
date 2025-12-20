namespace SchoolManagementSystem.Application.DTOs.Class
{
    public class EnrolledClassDto
    {
        public int ClassId { set; get; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string Semester { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly EnrollmentDate { set; get; }
    }
}
