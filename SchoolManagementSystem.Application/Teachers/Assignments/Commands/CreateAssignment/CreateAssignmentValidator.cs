using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment
{
    public class CreateAssignmentValidator:AbstractValidator<CreateAssignmentCommand>
    {
        public CreateAssignmentValidator()
        {
            RuleFor(x => x.CreateAssignmentDto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.CreateAssignmentDto.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.CreateAssignmentDto.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(x => x.CreateAssignmentDto.ClassId)
                .GreaterThan(0).WithMessage("ClassId must be greater than 0.");

            RuleFor(x => x.CreateAssignmentDto.CreatedByTeacherId)
                .NotEmpty().WithMessage("Teacher Id is required.");
        }
    }
}
