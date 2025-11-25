using FluentValidation;
using SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourse
{
    class CreateCourseValidator:AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseValidator()
        {
            RuleFor(x => x.CourseDto.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(100).WithMessage("Course name must not exceed 100 characters");

            RuleFor(x => x.CourseDto.Code)
              .NotEmpty().WithMessage("Course code is required")
              .MaximumLength(20).WithMessage("Course code must not exceed 20 characters");

            RuleFor(x => x.CourseDto.Description)
               .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.CourseDto.Credits)
               .GreaterThan(0).WithMessage("Credits must be greater than 0");
        }
    }
}
