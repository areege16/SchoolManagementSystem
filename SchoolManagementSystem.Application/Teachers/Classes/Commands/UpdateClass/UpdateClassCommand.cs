using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<ResponseDto<bool>>
    {
        public UpdateClassDto ClassDto { get; set; }
        public string TeacherId { get; set; }
    }
}
