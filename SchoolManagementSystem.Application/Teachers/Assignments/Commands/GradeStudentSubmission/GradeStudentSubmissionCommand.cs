using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission
{
    public class GradeStudentSubmissionCommand:IRequest<ResponseDto<bool>>
    {
        public ClaimsPrincipal User { get; set; }
        public GradeStudentSubmissionDto GradeStudentSubmissionDto { get; set; }
    }
}
