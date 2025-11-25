using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses
{
    public class GetAllCoursesQuery:IRequest<ResponseDto<List<CourseDto>>> 
    {
    }
}
