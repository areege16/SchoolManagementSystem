using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades
{
    public class GetStudentGradesQuery:IRequest<ResponseDto<List<GetStudentGradeDto>>>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
