using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Queries.GetAttendanceHistory
{
    public class GetAttendanceHistoryCommand:IRequest<ResponseDto<List<AttendanceHistoryDto>>>
    {
        public int ClassId { set; get; }
    }
}
