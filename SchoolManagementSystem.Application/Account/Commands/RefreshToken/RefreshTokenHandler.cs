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

namespace SchoolManagementSystem.Application.Account.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ResponseDto<TokenResponseDto>>
    {
        private readonly IGenericRepository<Domain.Models.RefreshToken> repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly JwtSettings jwtSettings;
        public RefreshTokenHandler(IGenericRepository<Domain.Models.RefreshToken> repository,
                                   UserManager<ApplicationUser> userManager,
                                   ITokenService tokenService,
                                   IOptions<JwtSettings> jwtSettings)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.jwtSettings = jwtSettings.Value;
        }
        public async Task<ResponseDto<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = tokenService.HashToken(request.RefreshTokenRequestDto.RefreshToken);

            var storedRefreshToken = await repository
            .GetFiltered(rt => rt.Token == hashedToken,tracked:true) 
            .Include(rt => rt.ApplicationUser)
            .FirstOrDefaultAsync(cancellationToken);

            if (storedRefreshToken == null)
            {
                return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Invalid refresh token.");
            }
            if (storedRefreshToken.IsRevoked)
            {
                return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Refresh token has been revoked.");
            }
            if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return ResponseDto<TokenResponseDto>.Error(ErrorCode.Unauthorized, "Refresh token has expired.");
            }

            var user = storedRefreshToken.ApplicationUser;
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
            await repository.SaveChangesAsync();

            var userRoles = await userManager.GetRolesAsync(user);
            var newAccessToken = tokenService.GenerateAccessToken(user, userRoles);
            var tokenResponseDto = new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
            return ResponseDto<TokenResponseDto>.Success(tokenResponseDto, "Token refreshed successfully.");
        }
    }
}
