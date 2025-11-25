using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses;
using SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse;
using SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById;
using SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses;
using SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse;

using SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment;
using SchoolManagementSystem.Application.DTOs.Course;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator mediator;
        public CoursesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region CreateCourse
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto courseDto)
        {
            var result = await mediator.Send(new CreateCourseCommand
            {
                CourseDto = courseDto
            });
            return Ok(result);
        }
        #endregion

        #region DeleteCourse
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await mediator.Send(new DeleteCourseCommand
            {
                Id= id
            });
            return Ok(result);
        }
        #endregion

        #region GetAllCourses
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await mediator.Send(new GetAllCoursesQuery());
            return Ok(result);
        }
        #endregion

        #region GetCourseById
        [HttpGet("GetCourseById")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await mediator.Send(new GetACourseByIdQuery{
                Id = id
            });
            return Ok(result);
        }
        #endregion

        #region UpdateCourse
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(int id , UpdateCourseDto courseDto)
        {
            courseDto.Id = id;
            var result = await mediator.Send(new UpdateCourseCommand
            {
                CourseDto=courseDto
            });
            return Ok(result);
        }
        #endregion
    }
}
