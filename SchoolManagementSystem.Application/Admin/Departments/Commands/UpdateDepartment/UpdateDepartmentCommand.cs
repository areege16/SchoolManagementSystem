using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment
{
   public class UpdateDepartmentCommand:IRequest<ResponseDto<bool>>
    {
        public UpdateDepartmentDto DepartmentDto { get; set; }
    }
}
