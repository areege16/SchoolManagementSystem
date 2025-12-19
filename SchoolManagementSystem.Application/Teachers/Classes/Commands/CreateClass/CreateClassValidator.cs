using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    class CreateClassValidator:AbstractValidator<CreateClassCommand>
    {
        private readonly IGenericRepository<Class> repository;

        public CreateClassValidator(IGenericRepository<Class> repository)
        {
            this.repository = repository;

            RuleFor(x => x.ClassDto.Name)
            .NotEmpty().WithMessage("Class name is required.")
            .MaximumLength(100).WithMessage("Class name must not exceed 100 characters.")
            .MustAsync(async(dto, name, cancellation) =>{
                var exists =await repository
                    .GetFiltered(c => c.Name == name && c.CourseId == dto.ClassDto.CourseId, asTracking: true)
                    .AnyAsync(cancellation);
                return !exists; 
            }).WithMessage("Class name already exists for this course.");

            RuleFor(x => x.ClassDto.StartDate)
             .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Start date cannot be in the past.")    
             .LessThan(x => x.ClassDto.EndDate).WithMessage("Start date must be before end date.");

            RuleFor(x => x.ClassDto.EndDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("End date must be today or in the future.");

            RuleFor(x => x.ClassDto.CourseId)
             .GreaterThan(0).WithMessage("Course must be selected.");

            RuleFor(x => x.ClassDto.Semester)
             .IsInEnum().WithMessage("Semester must be a valid value.");
        }
    }
}