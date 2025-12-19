using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses
{
    public class GetAllClassesCommand : IRequest<ResponseDto<List<ClassDto>>>
    {
        public string TeacherId { get; set; }
    }
}