using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.DTOs.Attendance.Teacher
{
    public class MarkAttendanceDto
    {
        public int ClassId { set; get; }
        public string StudentId { set; get; }
        public AttendanceStatus Status { set; get; }
    }
}