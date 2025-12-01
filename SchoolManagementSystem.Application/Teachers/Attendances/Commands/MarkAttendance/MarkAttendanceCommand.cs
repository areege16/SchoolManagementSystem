using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    public class MarkAttendanceCommand:IRequest<ResponseDto<bool>>
    {
        public MarkAttendanceDto AttendanceDto { get; set; }
    }
}
