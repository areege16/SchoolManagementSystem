using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass;
using SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass;
using SchoolManagementSystem.Application.Teachers.Classes.Commands.DeleteClass;
using SchoolManagementSystem.Application.Teachers.Classes.Commands.UpdateClass;
using SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.Teacher
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ClassesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region CreateClass
        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassDto classDto)
        {
            var teacherId = User.GetUserId();
            var result = await mediator.Send(new CreateClassCommand
            {
                ClassDto = classDto,
                TeacherId = teacherId,
            });
            return Ok(result);
        }
        #endregion

        #region GetAllClasses
        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            var teacherId = User.GetUserId();

            var result = await mediator.Send(new GetAllClassesCommand
            {
                TeacherId = teacherId
            });
            return Ok(result);
        }
        #endregion

        #region UpdateClass
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, UpdateClassDto classDto)
        {
            classDto.Id = id;
            var teacherId = User.GetUserId();

            var result = await mediator.Send(new UpdateClassCommand
            {
                ClassDto = classDto,
                TeacherId = teacherId,
            });
            return Ok(result);
        }
        #endregion

        #region DeleteClass
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var result = await mediator.Send(new DeleteClassCommand
            {
                Id = id
            });
            return Ok(result);
        }
        #endregion

        #region AssignStudentToClass
        [HttpPost("AssignStudentToClass")]
        public async Task<IActionResult> AssignStudentToClass(AssignStudentToClassDto assignStudentToClassDto)
        {
            var teacherId = User.GetUserId();
            var result = await mediator.Send(new AssignStudentToClassCommand
            {
                AssignStudentToClassDto = assignStudentToClassDto,
                TeacherId = teacherId
            });
            return Ok(result);
        }
        #endregion
    }
}