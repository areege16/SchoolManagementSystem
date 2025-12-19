using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.StudentControllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator mediator;

        public AttendanceController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentAttendance()
        {
            var studentId = User.GetUserId();
            var result = await mediator.Send(new GetAttendanceQuery
            {
                StudentId = studentId
            });
            return Ok(result);
        }
    }
}