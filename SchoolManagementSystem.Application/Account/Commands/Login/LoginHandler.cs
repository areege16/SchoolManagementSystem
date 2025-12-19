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
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Account.Commands.Login
{
    class LoginHandler : IRequestHandler<LoginCommand, ResponseDto<LoginResponseDto>>
    {
        private readonly IGenericRepository<Domain.Models.RefreshToken> repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILogger<LoginHandler> logger;
        private readonly JwtSettings jwtSettings;
        public LoginHandler(IGenericRepository<Domain.Models.RefreshToken> repository,
                            UserManager<ApplicationUser> userManager,
                            ITokenService tokenService,
                            IOptions<JwtSettings> jwtSettings,
                            ILogger<LoginHandler> logger)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
            this.jwtSettings = jwtSettings.Value;
        }
        public async Task<ResponseDto<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Login attempt for user {UserName}", request.LoginDto.UserName);

                ApplicationUser user = await userManager.FindByNameAsync(request.LoginDto.UserName);

                if (user == null)
                {
                    logger.LogWarning("Login failed: user not found {UserName}", request.LoginDto.UserName);
                    return ResponseDto<LoginResponseDto>.Error(ErrorCode.NotFound, "User not found");
                }

                bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.LoginDto.Password);
                if (!isPasswordCorrect)
                {
                    logger.LogWarning("Login failed: invalid password for {UserName}", request.LoginDto.UserName);
                    return ResponseDto<LoginResponseDto>.Error(ErrorCode.Unauthorized, "Invalid username or password");
                }
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
                await repository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Login successful for user {UserName}", request.LoginDto.UserName);

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
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred during login for {UserName}", request.LoginDto.UserName);
                return ResponseDto<LoginResponseDto>.Error(ErrorCode.InternalServerError, "An unexpected error occurred during login.");
            }
        }
    }
}