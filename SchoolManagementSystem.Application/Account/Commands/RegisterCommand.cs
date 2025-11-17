using MediatR;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Account;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Account.Commands
{
    public class RegisterCommand:IRequest<ResponseDto<bool>>
    {
        public RegisterDto UserFormConsumer { get; set; }
    }
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseDto<bool>>
    {

        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
          "Admin",
          "Teacher",
          "Student"
        };

        private readonly UserManager<ApplicationUser> userManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ResponseDto<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var userForm =request.UserFormConsumer;

            if (!string.IsNullOrEmpty(userForm.Role) && !AllowedRoles.Contains(userForm.Role))
            {
                return ResponseDto<bool>.Error(ErrorCode.InvalidRole, "Invalid role specified. Allowed roles are: Admin, Teacher, Student.");
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName =userForm.UserName,
                Name=userForm.Name,
                Email= userForm.Email,
            };

            IdentityResult result = await userManager.CreateAsync(user,userForm.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<bool>.Error(ErrorCode.Unauthorized, $"Account creation failed: {errors}");
            }

            if (!string.IsNullOrEmpty(userForm.Role))
            {
                var isInRole = await userManager.IsInRoleAsync(user, userForm.Role);

                if (!isInRole)
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, userForm.Role);
                    if (!addToRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return ResponseDto<bool>.Error(ErrorCode.Unauthorized, $"Failed to assign role{errors}");
                    }
                }
            }
            return ResponseDto<bool>.Success(true, "Account Created successfully");

        }
    }
}
