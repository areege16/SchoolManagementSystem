using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById
{
    public class GetAssignmentDetailsQuery : IRequest<ResponseDto<AssignmentDetailsDto>>
    {
        public int AssignmentId { set; get; }
        public string TeacherId { set; get; }
    }
}
