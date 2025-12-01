using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;
using SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance;
using SchoolManagementSystem.Application.Teachers.Attendances.Queries.GetAttendanceHistory;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.TeacherControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles= "Teacher")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator mediator;

        public AttendanceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region MarkAttendance
        [HttpPost]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceDto attendanceDto)
        {
            var result = await mediator.Send(new MarkAttendanceCommand
            {
                AttendanceDto = attendanceDto
            });
            return Ok(result);
        }
        #endregion

        #region GetAttendanceHistory
        [HttpGet("{classId}")]
        public async Task<IActionResult> GetAttendanceHistory(int classId)
        {
            var result = await mediator.Send(new GetAttendanceHistoryCommand
            {
               ClassId= classId
            });
            return Ok(result);
        }
        #endregion
    }
}
