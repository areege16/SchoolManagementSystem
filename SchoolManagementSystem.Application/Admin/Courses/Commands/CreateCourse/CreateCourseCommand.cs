using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses
{
    public class CreateCourseCommand : IRequest<ResponseDto<bool>>
    {
        public CreateCourseDto CourseDto { get; set; }
        public string AdminId { get; set; }
    }
}
