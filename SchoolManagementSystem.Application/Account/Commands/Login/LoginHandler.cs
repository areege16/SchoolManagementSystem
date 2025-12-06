using MediatR;
using SchoolManagementSystem.Application.DTOs.Account;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolManagementSystem.Application.Account.Commands.Login
{
    class LoginHandler : IRequestHandler<LoginCommand, ResponseDto<LoginResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configur;

        public LoginHandler(UserManager<ApplicationUser> userManager, IConfiguration configur)
        {
            this.userManager = userManager;
            this.configur = configur;
        }
        public async Task<ResponseDto<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            ApplicationUser user = await userManager.FindByNameAsync(request.LoginDto.UserName);

            if (user == null)
                return ResponseDto<LoginResponseDto>.Error(ErrorCode.NotFound, "User not found");

            bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.LoginDto.Password);

            if (!isPasswordCorrect)
                return ResponseDto<LoginResponseDto>.Error(ErrorCode.Unauthorized, "Invalid username or password");

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

            var loginDto = new LoginResponseDto
            {
                UserName = user.UserName,
                Name = user.Name,
                Role = userRoles.FirstOrDefault(),
                Token = generatedToken,
                ExpireAt = expiresAt
            };

            return ResponseDto<LoginResponseDto>.Success(loginDto, "Login successful");
        }
    }
}