using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommand:IRequest<ResponseDto<bool>>
    {
        public UpdateCourseDto CourseDto { get; set; }
    }
}
