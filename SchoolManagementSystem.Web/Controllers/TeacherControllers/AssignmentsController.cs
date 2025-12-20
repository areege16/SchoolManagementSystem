using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment;
using SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission;
using SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById;
using SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetTeacherAssignments;
using SchoolManagementSystem.Web.Extensions;
namespace SchoolManagementSystem.Web.Controllers.TeacherControllers
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
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
            var teacherId = User.GetUserId();

            var result = await mediator.Send(new CreateAssignmentCommand
            {
                CreateAssignmentDto = createAssignmentDto,
                TeacherId = teacherId
            });
            return Ok(result);
        }
        #endregion

        #region GetAssignmentById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            var teacherId = User.GetUserId();
            var result = await mediator.Send(new GetAssignmentDetailsQuery
            {
                AssignmentId = id,
                TeacherId = teacherId,
            });
            return Ok(result);
        }
        #endregion

        #region GradeStudentSubmission
        [HttpPost("{id}/grade")]
        public async Task<IActionResult> GradeStudentSubmission(int id, GradeStudentSubmissionDto gradeStudentSubmissionDto)
        {
            gradeStudentSubmissionDto.AssignmentId = id;
            var teacherId = User.GetUserId();
            var result = await mediator.Send(new GradeStudentSubmissionCommand
            {
                GradeStudentSubmissionDto = gradeStudentSubmissionDto,
                TeacherId = teacherId,
            });
            return Ok(result);
        }
        #endregion

        #region GetTeacherAssignments
        [HttpGet]
        public async Task<IActionResult> GetTeacherAssignments()
        {
            var teacherId = User.GetUserId();
            var result = await mediator.Send(new GetTeacherAssignmentsQuery
            {
                TeacherId = teacherId,
            });
            return Ok(result);
        }
        #endregion
    }
}