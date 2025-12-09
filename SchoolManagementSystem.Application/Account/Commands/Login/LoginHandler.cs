using MediatR;
using SchoolManagementSystem.Application.DTOs.Account;
using SchoolManagementSystem.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Application.Services.TokenService;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.Settings;

namespace SchoolManagementSystem.Application.Account.Commands.Login
{
    class LoginHandler : IRequestHandler<LoginCommand, ResponseDto<LoginResponseDto>>
    {
        private readonly IGenericRepository<Domain.Models.RefreshToken> repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly JwtSettings jwtSettings;
        public LoginHandler(IGenericRepository<Domain.Models.RefreshToken> repository,
                            UserManager<ApplicationUser> userManager,
                            ITokenService tokenService,
                            IOptions<JwtSettings> jwtSettings)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.jwtSettings = jwtSettings.Value;
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
            var accessToken = tokenService.GenerateAccessToken(user, userRoles);
            var refreshToken = tokenService.GenerateRefreshToken();
            var hashedRefreshToken = tokenService.HashToken(refreshToken);

            var refreshTokenEntity = new Domain.Models.RefreshToken
            {
                Token = hashedRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false      
            };
            repository.Add(refreshTokenEntity);
            await repository.SaveChangesAsync();

            var loginDto = new LoginResponseDto
            {
                UserName = user.UserName,
                Name = user.Name,
                Role = userRoles.FirstOrDefault(),
                Token = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
            };
            return ResponseDto<LoginResponseDto>.Success(loginDto, "Login successful");
        }
    }
}