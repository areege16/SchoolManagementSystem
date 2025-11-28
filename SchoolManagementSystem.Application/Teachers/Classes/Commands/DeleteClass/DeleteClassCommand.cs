using MediatR;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.DeleteClass
{
    public class DeleteClassCommand:IRequest<ResponseDto<bool>>
    {
        public int Id { set; get; }
    }
}
