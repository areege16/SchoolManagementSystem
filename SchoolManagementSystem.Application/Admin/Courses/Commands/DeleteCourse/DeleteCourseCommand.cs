using MediatR;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommand:IRequest<ResponseDto<bool>>
    {
        public int Id { get; set; }
    }
}
