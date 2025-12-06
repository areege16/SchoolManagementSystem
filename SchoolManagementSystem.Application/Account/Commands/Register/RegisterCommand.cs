using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Account.Commands.Register
{
    public class RegisterCommand : IRequest<ResponseDto<bool>>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}