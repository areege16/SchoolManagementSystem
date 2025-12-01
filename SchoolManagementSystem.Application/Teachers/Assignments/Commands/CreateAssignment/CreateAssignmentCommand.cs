using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment
{
   public class CreateAssignmentCommand:IRequest<ResponseDto<bool>>
    {
        public CreateAssignmentDto CreateAssignmentDto { get; set; }
    }
}
