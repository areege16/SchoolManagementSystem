using FluentValidation;


namespace SchoolManagementSystem.Application.Account.Commands.Login
{
    class LoginValidator:AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.LoginDto.UserName).NotEmpty()
                .WithMessage("Username is required");

            RuleFor(x => x.LoginDto.Password).NotEmpty()
                .WithMessage("Password is required");
        }
    }
}