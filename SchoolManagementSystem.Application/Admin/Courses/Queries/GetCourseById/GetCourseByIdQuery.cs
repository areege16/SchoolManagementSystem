using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById
{
    public class GetCourseByIdQuery : IRequest<ResponseDto<CourseDto>>
    {
        public int Id { get; set; }
    }
}