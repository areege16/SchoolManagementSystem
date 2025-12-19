using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<ResponseDto<bool>>
    {
        public CreateClassDto ClassDto { get; set; }
        public string TeacherId { get; set; }
    }
}
