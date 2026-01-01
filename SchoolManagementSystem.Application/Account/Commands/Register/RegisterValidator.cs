using FluentValidation;

namespace SchoolManagementSystem.Application.Account.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.RegisterDto.UserName).NotEmpty();

            RuleFor(x => x.RegisterDto.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.RegisterDto.Password).NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password should be at least 6 characters ");

            RuleFor(x => x.RegisterDto.ConfirmPassword).Equal(x => x.RegisterDto.Password)
                .WithMessage("Passwords do not match");

            RuleFor(x => x.RegisterDto.Role).NotEmpty();
        }
    }
}