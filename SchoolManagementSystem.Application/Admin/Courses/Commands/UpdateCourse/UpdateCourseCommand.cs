using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<ResponseDto<bool>>
    {
        public UpdateCourseDto CourseDto { get; set; }
        public string AdminId { get; set; }
    }
}