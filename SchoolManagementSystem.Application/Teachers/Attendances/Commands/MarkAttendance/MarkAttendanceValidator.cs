using FluentValidation;
using SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    class MarkAttendanceValidator:AbstractValidator<MarkAttendanceCommand>
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

            RuleFor(x => x.AttendanceDto.MarkedByTeacherId)
                .NotEmpty()
                .WithMessage("MarkedByTeacherId is required.");

            RuleFor(x => x.AttendanceDto.Date)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Attendance date cannot be in the future.");
        }
    }
}
