using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Account;

namespace SchoolManagementSystem.Application.Account.Commands.Register
{
    public class RegisterCommand : IRequest<ResponseDto<bool>>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}