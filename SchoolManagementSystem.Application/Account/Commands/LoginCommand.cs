using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Account;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Account.Commands
{

    public class LoginCommand : IRequest<ResponseDto<LoginDto>> //TODO Revisit and make Validation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseDto<LoginDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configur;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configur)
        {
            this.userManager = userManager;
            this.configur = configur;
        }
        async Task<ResponseDto<LoginDto>> IRequestHandler<LoginCommand, ResponseDto<LoginDto>>.Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return ResponseDto<LoginDto>.Error(ErrorCode.NotFound, "User not found");


            bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
                return ResponseDto<LoginDto>.Error(ErrorCode.Unauthorized, "Invalid username or password");


            var userRoles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey signinkey = new(Encoding.UTF8.GetBytes(configur["JWT:Key"]));

            SigningCredentials signingCredentials =
                new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
                        issuer: configur["JWT:Iss"],
                        audience: configur["JWT:Aud"],
                        expires: expiresAt,
                        claims: claims,
                        signingCredentials: signingCredentials
             );

            string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var loginDto = new LoginDto
            {
                UserName = user.UserName,
                Name = user.Name,
                Role = userRoles.FirstOrDefault(),
                Token = generatedToken,
                ExpireAt = expiresAt
            };

            return ResponseDto<LoginDto>.Success(loginDto, "Login successful");
        }
    }
}





