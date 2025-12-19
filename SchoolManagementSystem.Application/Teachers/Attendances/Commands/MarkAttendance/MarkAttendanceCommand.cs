using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    public class MarkAttendanceCommand : IRequest<ResponseDto<bool>>
    {
        public MarkAttendanceDto AttendanceDto { get; set; }
        public string TeacherId { get; set; }
    }
}
