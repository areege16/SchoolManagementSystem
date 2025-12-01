using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment;
using SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission;
using SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.TeacherControllers
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    [Authorize(Roles ="Teacher")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AssignmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region CreateAssignment
        [HttpPost]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentDto createAssignmentDto)
        {
            var result = await mediator.Send(new CreateAssignmentCommand
            {
                CreateAssignmentDto = createAssignmentDto
            });
            return Ok(result);
        }
        #endregion

        #region GetAssignmentById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            var result = await mediator.Send(new GetAssignmentByIdQuery
            {
                Id=id
            });
            return Ok(result);
        }
        #endregion

        #region GradeStudentSubmission
        [HttpPost("{id}/grade")]
        public async Task<IActionResult> GradeStudentSubmission(int id,GradeStudentSubmissionDto gradeStudentSubmissionDto )
        {
            gradeStudentSubmissionDto.AssignmentId = id;
            var result = await mediator.Send(new GradeStudentSubmissionCommand
            {
                User = User,
                GradeStudentSubmissionDto = gradeStudentSubmissionDto
            });
            return Ok(result);
        }
        #endregion
    }
}
