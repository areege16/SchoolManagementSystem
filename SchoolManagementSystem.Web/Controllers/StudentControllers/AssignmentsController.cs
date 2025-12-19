using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment;
using SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentAssignments;
using SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades;
using SchoolManagementSystem.Web.Extensions;

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
        #region GetStudentAssignments
        [HttpGet]
        public async Task<IActionResult> GetStudentAssignments()
        {
            var studentId = User.GetUserId();

            var result = await mediator.Send(new GetStudentAssignmentsQuery
            {
                StudentId = studentId,
            });
            return Ok(result);
        }
        #endregion

        #region SubmitAssignment
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitAssignment(int id, [FromForm] SubmitAssignmentDto submitAssignmentDto)
        {
            var studentId = User.GetUserId();
            submitAssignmentDto.AssignmentId = id;
            var result = await mediator.Send(new SubmitAssignmentCommand
            {
                StudentId = studentId,
                SubmitAssignmentDto = submitAssignmentDto
            });
            return Ok(result);
        }
        #endregion

        #region GetStudentGrades
        [HttpGet("grads")]
        public async Task<IActionResult> GetStudentGrades()
        {
            var studentId = User.GetUserId();
            var result = await mediator.Send(new GetStudentGradesQuery
            {
                studentId = studentId,
            });
            return Ok(result);
        }
        #endregion
    }
}