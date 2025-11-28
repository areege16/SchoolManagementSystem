using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass
{
    class AssignStudentToClassValidator :AbstractValidator<AssignStudentToClassCommand>
    {
        public AssignStudentToClassValidator()
        {
            RuleFor(x => x.AssignStudentToClassDto.StudentId)
            .NotEmpty()
            .WithMessage("StudentId is required.");

            RuleFor(x => x.AssignStudentToClassDto.ClassId)
            .GreaterThan(0)
            .WithMessage("ClassId must be greater than 0.");
        }
    }
}
