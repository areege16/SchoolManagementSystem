using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses
{
    public class CreateCourseCommand:IRequest<ResponseDto<bool>>
    {
       public CreateCourseDto CourseDto { get; set; }
    }
}
