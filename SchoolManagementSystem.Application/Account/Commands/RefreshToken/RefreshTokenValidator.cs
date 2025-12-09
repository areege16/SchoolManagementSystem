using FluentValidation;

namespace SchoolManagementSystem.Application.Account.Commands.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshTokenRequestDto.RefreshToken)
               .NotNull()
               .WithMessage("Refresh token is required.")
               .NotEmpty()
               .WithMessage("Refresh token cannot be empty.")
               .MinimumLength(32)
               .WithMessage("Refresh token is too short.")
               .MaximumLength(256)
               .WithMessage("Refresh token is too long.");
        }
    }
}
