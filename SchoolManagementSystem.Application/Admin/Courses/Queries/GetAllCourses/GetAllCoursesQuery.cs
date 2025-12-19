using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses
{
    public class GetAllCoursesQuery : IRequest<ResponseDto<List<CourseDto>>>
    {
    }
}