using MediatR;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
   public class DeleteDepartmentCommand:IRequest<ResponseDto<bool>>
    {
        public int Id { get; set; }
    }
}
