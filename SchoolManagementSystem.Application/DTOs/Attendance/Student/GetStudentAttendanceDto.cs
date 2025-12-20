using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.DTOs.Attendance.Student
{
   public class GetStudentAttendanceDto
    {
        public int ClassId { set; get; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public DateOnly Date { set; get; }
        public AttendanceStatus Status { set; get; }
        public string MarkedByTeacherName { get; set; }
    }
}