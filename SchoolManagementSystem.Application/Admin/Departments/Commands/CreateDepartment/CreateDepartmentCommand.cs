using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment
{
   public class CreateDepartmentCommand:IRequest<ResponseDto<bool>>
    {
        public CreateDepartmentDto DepartmentDto { get; set; } 
    }
}
