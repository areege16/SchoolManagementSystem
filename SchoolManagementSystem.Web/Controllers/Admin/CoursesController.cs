using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses;
using SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse;
using SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById;
using SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses;
using SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator mediator;
        public CoursesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region CreateNewCourse
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto courseDto)
        {
            var adminId = User.GetUserId();
            var result = await mediator.Send(new CreateCourseCommand
            {
                CourseDto = courseDto,
                AdminId = adminId,
            });
            return Ok(result);
        }
        #endregion

        #region DeleteCourse
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await mediator.Send(new DeleteCourseCommand
            {
                Id = id
            });
            return Ok(result);
        }
        #endregion

        #region GetAllCourses
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await mediator.Send(new GetAllCoursesQuery());
            return Ok(result);
        }
        #endregion

        #region GetCourseById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await mediator.Send(new GetCourseByIdQuery
            {
                Id = id
            });
            return Ok(result);
        }
        #endregion

        #region UpdateCourse
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto courseDto)
        {
            courseDto.Id = id;
            var adminId = User.GetUserId();

            var result = await mediator.Send(new UpdateCourseCommand
            {
                CourseDto = courseDto,
                AdminId = adminId
            });
            return Ok(result);
        }
        #endregion
    }
}