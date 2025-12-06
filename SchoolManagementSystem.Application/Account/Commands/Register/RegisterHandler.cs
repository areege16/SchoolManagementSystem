using MediatR;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;


namespace SchoolManagementSystem.Application.Account.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseDto<bool>>
    {
        private static readonly HashSet<string> AllowedRolesSet = new(Roles.AllowedRoles,StringComparer.OrdinalIgnoreCase);
     

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepository<Domain.Models.Admin> adminRepository;
        private readonly IGenericRepository<Teacher> teacherRepository;
        private readonly IGenericRepository<Student> studentRepository;

        public RegisterHandler(UserManager<ApplicationUser> userManager,
                               IGenericRepository<Domain.Models.Admin> adminRepository,
                               IGenericRepository<Teacher> teacherRepository,
                               IGenericRepository<Student> studentRepository)
        {
            this.userManager = userManager;
            this.adminRepository = adminRepository;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
        }
        public async Task<ResponseDto<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userForm = request.RegisterDto;

            if (!string.IsNullOrEmpty(userForm.Role) && !AllowedRolesSet.Contains(userForm.Role))
            {
                return ResponseDto<bool>.Error(ErrorCode.InvalidRole, "Invalid role specified. Allowed roles are: Admin, Teacher, Student.");
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = userForm.UserName,
                Name = userForm.Name,
                Email = userForm.Email,
            };

            IdentityResult result = await userManager.CreateAsync(user, userForm.Password);

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
                        var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                        return ResponseDto<bool>.Error(ErrorCode.Unauthorized, $"Failed to assign role{errors}");
                    }
                }

                switch (userForm.Role.ToLower())
                {
                    case "admin":
                        var admin = new Domain.Models.Admin { Id = user.Id };
                        adminRepository.Add(admin);
                        await adminRepository.SaveChangesAsync();
                        break;

                    case "teacher":
                        var teacher = new Teacher { Id = user.Id };
                        teacherRepository.Add(teacher);
                        await teacherRepository.SaveChangesAsync();
                        break;

                    case "student":
                        var student = new Student { Id = user.Id };
                        studentRepository.Add(student);
                        await studentRepository.SaveChangesAsync();
                        break;
                }
            }
            return ResponseDto<bool>.Success(true, "Account Created successfully");
        }
    }
}