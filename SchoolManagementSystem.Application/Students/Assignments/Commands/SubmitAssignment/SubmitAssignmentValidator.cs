using FluentValidation;

namespace SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment
{
    class SubmitAssignmentValidator : AbstractValidator<SubmitAssignmentCommand>
    {
        public SubmitAssignmentValidator()
        {

            RuleFor(x => x.SubmitAssignmentDto.File)
                .NotNull()
                .WithMessage("File is required.")
                .Must(file => file.Length > 0)
                .WithMessage("File cannot be empty.");
        }
    }
}