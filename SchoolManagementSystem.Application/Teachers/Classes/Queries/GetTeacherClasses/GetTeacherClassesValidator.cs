using FluentValidation;
using SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetTeacherClasses
{
    class GetTeacherClassesValidator : AbstractValidator<GetTeacherClassesQuery>
    {
        public GetTeacherClassesValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("Page size must be between 1 and 50");
        }
    }
}