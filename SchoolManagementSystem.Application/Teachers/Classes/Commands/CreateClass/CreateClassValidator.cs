using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    class CreateClassValidator:AbstractValidator<CreateClassCommand>
    {
        private readonly IGenericRepository<Class> repository;

        public CreateClassValidator(IGenericRepository<Class> repository)
        {
            this.repository = repository;

            RuleFor(x => x.classDto.Name)
            .NotEmpty().WithMessage("Class name is required.")
            .MaximumLength(100).WithMessage("Class name must not exceed 100 characters.")
            .MustAsync(async(dto, name, cancellation) =>{
                var exists =await repository
                    .GetFiltered(c => c.Name == name && c.CourseId == dto.classDto.CourseId, tracked: true)
                    .AnyAsync(cancellation);
                return !exists; 
            }).WithMessage("Class name already exists for this course.");

            RuleFor(x => x.classDto.StartDate)
             .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Start date cannot be in the past.")    
             .LessThan(x => x.classDto.EndDate).WithMessage("Start date must be before end date.");

            RuleFor(x => x.classDto.EndDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("End date must be today or in the future.");

            RuleFor(x => x.classDto.CourseId)
             .GreaterThan(0).WithMessage("Course must be selected.");

            RuleFor(x => x.classDto.Semester)
             .IsInEnum().WithMessage("Semester must be a valid value.");
        }
    }
}
