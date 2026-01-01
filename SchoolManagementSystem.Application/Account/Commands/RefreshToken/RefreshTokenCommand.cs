using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Account;

namespace SchoolManagementSystem.Application.Account.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ResponseDto<TokenResponseDto>>
    {
        public RefreshTokenRequestDto RefreshTokenRequestDto { get; set; }
    }
}