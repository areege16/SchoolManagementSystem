using FluentValidation;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    class MarkAttendanceValidator : AbstractValidator<MarkAttendanceCommand>
    {
        public MarkAttendanceValidator()
        {
            RuleFor(x => x.AttendanceDto.Status)
                .IsInEnum()
                .WithMessage("Status must be a valid value : Present, Absent, Late.");

            RuleFor(x => x.AttendanceDto.ClassId)
              .GreaterThan(0)
              .WithMessage("ClassId must be greater than zero.")
              .NotEmpty()
              .WithMessage("ClassId is required.");

            RuleFor(x => x.AttendanceDto.StudentId)
                .NotEmpty()
                .WithMessage("StudentId is required.");
        }
    }
}