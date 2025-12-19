using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById
{
    public class GetAssignmentByIdQuery : IRequest<ResponseDto<AssignmentDto>>
    {
        public int AssignmentId { set; get; }
        public string TeacherId { set; get; }
    }
}
