using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    public class GetEnrolledClassesCommand : IRequest<ResponseDto<List<EnrolledClassDto>>>
    {
        public string StudentId { get; set; }
    }
}