using FluentValidation;
using SchoolManagementSystem.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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