using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById
{
    public class GetACourseByIdQuery:IRequest<ResponseDto<CourseDto>>
    {
        public int Id { get; set; }
    }
}
