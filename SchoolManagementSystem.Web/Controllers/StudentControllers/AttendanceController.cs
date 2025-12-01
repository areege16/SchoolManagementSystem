using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance;
using SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses;

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
        public async Task<IActionResult> GetAttendance()
        {
            var result = await mediator.Send(new GetAttendanceQuery
            {
                User = User
            });
            return Ok(result);
        }
    }
}
