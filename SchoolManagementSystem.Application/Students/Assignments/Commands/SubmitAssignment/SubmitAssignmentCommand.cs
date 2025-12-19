using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;

namespace SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment
{
    public class SubmitAssignmentCommand : IRequest<ResponseDto<bool>>
    {
        public SubmitAssignmentDto SubmitAssignmentDto { get; set; }
        public string StudentId { get; set; }
    }
}