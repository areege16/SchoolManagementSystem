using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment
{
    public class CreateAssignmentCommand : IRequest<ResponseDto<bool>>
    {
        public CreateAssignmentDto CreateAssignmentDto { get; set; }
        public string TeacherId { get; set; }
    }
}
