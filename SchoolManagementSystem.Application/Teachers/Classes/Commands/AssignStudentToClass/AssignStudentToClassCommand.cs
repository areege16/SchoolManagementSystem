using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass
{
    public class AssignStudentToClassCommand : IRequest<ResponseDto<bool>>
    {
        public AssignStudentToClassDto AssignStudentToClassDto { get; set; }
        public string TeacherId { get; set; }
    }
}