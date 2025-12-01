using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment;
using SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentAssignments;
using SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.StudentControllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AssignmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region GetAssignments
        [HttpGet]
        public async Task<IActionResult> GetAssignments()
        {
            var result = await mediator.Send(new GetStudentAssignmentsQuery
            {
                User = User
            });
            return Ok(result);
        }
        #endregion

        #region SubmitAssignment
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitAssignment(int id , [FromForm] SubmitAssignmentDto submitAssignmentDto)
        {
            submitAssignmentDto.AssignmentId = id;
            var result = await mediator.Send(new SubmitAssignmentCommand
            {
                User = User,
                SubmitAssignmentDto = submitAssignmentDto
            });
            return Ok(result);
        }
        #endregion

        #region GetStudentGrades
        [HttpGet("grads")]
        public async Task<IActionResult> GetStudentGrades()
        {
            var result = await mediator.Send(new GetStudentGradesQuery
            {
                User = User,
            });
            return Ok(result);
        }
        #endregion
    }
}
