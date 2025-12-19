using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.DTOs.Attendance.Teacher
{
    public class AttendanceHistoryDto
    {
        public int ClassId { set; get; }
        public string StudentId { set; get; }
        public string StudentName { set; get; }
        public string MarkedByTeacherId { get; set; }
        public AttendanceStatus Status { set; get; }
        public DateTime Date { set; get; }
    }
}