using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass
{
   public class AssignStudentToClassCommand:IRequest<ResponseDto<bool>>
    {
        public AssignStudentToClassDto AssignStudentToClassDto { get; set; } 
    }
}
