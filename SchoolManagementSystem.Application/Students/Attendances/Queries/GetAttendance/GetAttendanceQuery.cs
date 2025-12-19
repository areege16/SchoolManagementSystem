using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance.Student;

namespace SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance
{
    public class GetAttendanceQuery : IRequest<ResponseDto<List<GetStudentAttendanceDto>>>
    {
        public string StudentId { get; set; }
    }
}