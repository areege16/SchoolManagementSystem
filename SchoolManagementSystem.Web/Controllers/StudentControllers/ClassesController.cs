using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.StudentControllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator mediator;
        public ClassesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentEnrolledClasses()
        {
            var studentId = User.GetUserId();

            var result = await mediator.Send(new GetEnrolledClassesCommand
            {
                StudentId = studentId,
            });
            return Ok(result);
        }
    }
}