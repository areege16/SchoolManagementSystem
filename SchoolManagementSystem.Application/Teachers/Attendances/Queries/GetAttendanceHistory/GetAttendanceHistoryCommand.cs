using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Queries.GetAttendanceHistory
{
    public class GetAttendanceHistoryCommand : IRequest<ResponseDto<List<AttendanceHistoryDto>>>
    {
        public int ClassId { set; get; }
        public string TeacherId { get; set; }
    }
}