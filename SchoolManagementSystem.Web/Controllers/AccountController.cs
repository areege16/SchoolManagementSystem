using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Account.Commands;
using SchoolManagementSystem.Application.DTOs.Account;

namespace SchoolManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await mediator.Send(new RegisterCommand
            {
                UserFormConsumer = registerDto
            });
            return Ok(result);
        }
    }
}
