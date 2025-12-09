using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Account.Commands.Login;
using SchoolManagementSystem.Application.Account.Commands.RefreshToken;
using SchoolManagementSystem.Application.Account.Commands.Register;
using SchoolManagementSystem.Application.DTOs.Account;

namespace SchoolManagementSystem.Web.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await mediator.Send(new RegisterCommand
            {
                RegisterDto = registerDto
            });
            return Ok(result);
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await mediator.Send(new LoginCommand
            {
                LoginDto = loginDto
            });
            return Ok(result);
        }
        #endregion

        #region RefreshToken
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await mediator.Send(new RefreshTokenCommand
            {
                RefreshTokenRequestDto = refreshTokenRequestDto
            });
            return Ok(result);
        }
        #endregion
    }
}

