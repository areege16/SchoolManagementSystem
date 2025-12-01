using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById;
using SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.StudentControllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator mediator;
        public ClassesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetEnrolledClasses()
        {
            var result = await mediator.Send(new GetEnrolledClassesCommand
            {
                User = User
            });
            return Ok(result);
        }
    }   
}
