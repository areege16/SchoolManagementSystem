using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment
{
    public class SubmitAssignmentCommand:IRequest<ResponseDto<bool>>
    {
        public ClaimsPrincipal User { get; set; }
        public SubmitAssignmentDto SubmitAssignmentDto { get; set; }
    }
}
