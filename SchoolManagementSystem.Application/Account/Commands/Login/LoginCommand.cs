using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Account;

namespace SchoolManagementSystem.Application.Account.Commands.Login
{
    public class LoginCommand : IRequest<ResponseDto<LoginResponseDto>>
    {
        public LoginDto LoginDto { get; set; }
    }
}