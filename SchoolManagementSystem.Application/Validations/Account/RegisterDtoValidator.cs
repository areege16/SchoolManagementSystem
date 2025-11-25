using FluentValidation;
using SchoolManagementSystem.Application.DTOs.Account;


namespace SchoolManagementSystem.Application.Validations.Account
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto> 
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password should be at least 6 characters ");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
