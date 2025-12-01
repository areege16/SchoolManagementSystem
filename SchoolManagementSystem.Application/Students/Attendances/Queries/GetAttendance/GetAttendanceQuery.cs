using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance
{
    public class GetAttendanceQuery : IRequest<ResponseDto<List<GetStudentAttendanceDto>>>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
