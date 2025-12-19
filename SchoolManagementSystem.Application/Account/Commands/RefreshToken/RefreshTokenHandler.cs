using MediatR;
using SchoolManagementSystem.Application.DTOs.Account;
using SchoolManagementSystem.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.RepositoryContract;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Application.Services.TokenService;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.Settings;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Account.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ResponseDto<TokenResponseDto>>
    {
        private readonly IGenericRepository<Domain.Models.RefreshToken> repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILogger<RefreshTokenHandler> logger;
        private readonly JwtSettings jwtSettings;
        public RefreshTokenHandler(IGenericRepository<Domain.Models.RefreshToken> repository,
                                   UserManager<ApplicationUser> userManager,
                                   ITokenService tokenService,
                                   IOptions<JwtSettings> jwtSettings,
                                   ILogger<RefreshTokenHandler> logger)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
            this.jwtSettings = jwtSettings.Value;
        }
        public async Task<ResponseDto<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Refresh token attempt received");

                var hashedToken = tokenService.HashToken(request.RefreshTokenRequestDto.RefreshToken);

                var storedRefreshToken = await repository
                .GetFiltered(rt => rt.Token == hashedToken, asTracking: true)
                .Include(rt => rt.ApplicationUser)
                .FirstOrDefaultAsync(cancellationToken);


                if (storedRefreshToken == null)
                {
                    logger.LogWarning("Refresh token attempt failed: token not found");
                    return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Invalid refresh token.");
                }

                var user = storedRefreshToken.ApplicationUser;
                if (storedRefreshToken.IsRevoked)
                {
                    logger.LogWarning("Revoked refresh token used by user: {UserName}", user.UserName);
                    return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Refresh token has been revoked.");
                }
                if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
                {
                    logger.LogWarning("Expired refresh token used by user: {UserName}", user.UserName);
                    return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Refresh token has expired.");
                }

                storedRefreshToken.IsRevoked = true;
                storedRefreshToken.RevokedAt = DateTime.UtcNow;

                var newRefreshToken = tokenService.GenerateRefreshToken();
                var hashedNewRefreshToken = tokenService.HashToken(newRefreshToken);
                var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);

                var newRefreshTokenEntity = new Domain.Models.RefreshToken
                {
                    Token = hashedNewRefreshToken,
                    UserId = user.Id,
                    ExpiresAt = refreshTokenExpiresAt,
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };
                repository.Add(newRefreshTokenEntity);
                await repository.SaveChangesAsync(cancellationToken);

                var userRoles = await userManager.GetRolesAsync(user);
                var newAccessToken = tokenService.GenerateAccessToken(user, userRoles);

                logger.LogInformation("Refresh token successful for user {UserName}", user.UserName);

                var tokenResponseDto = new TokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiresAt = refreshTokenExpiresAt
                };
                return ResponseDto<TokenResponseDto>.Success(tokenResponseDto, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred during refresh token attempt");
                return ResponseDto<TokenResponseDto>.Error(ErrorCode.InternalServerError, "An unexpected error occurred during refresh token.");
            }
        }
    }
}