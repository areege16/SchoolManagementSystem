using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentValidator(IGenericRepository<Department> departmentRepository, IGenericRepository<Teacher> teacherRepository)
        {
            RuleFor(x => x.DepartmentDto.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MustAsync(async (name, cancellation) =>
                {
                    var exists = await departmentRepository
                    .GetFiltered(deptName => deptName.Name == name, asTracking: false)
                    .AnyAsync(cancellation);
                    return !exists;

                }).WithMessage("Department name must be unique.");


            RuleFor(x => x.DepartmentDto.HeadOfDepartmentId)
                .MustAsync(async (id, Cancellation) =>
                {
                    if (string.IsNullOrEmpty(id))
                        return true;

                    return await teacherRepository.GetAllAsNoTracking()
                    .AnyAsync(t => t.Id == id);

                }).WithMessage("Head of Department must be a valid teacher");

        }
    }
}