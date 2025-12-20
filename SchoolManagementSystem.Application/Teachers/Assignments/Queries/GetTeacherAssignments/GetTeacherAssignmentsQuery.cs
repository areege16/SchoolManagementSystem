using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetTeacherAssignments
{
    public class GetTeacherAssignmentsQuery : IRequest<ResponseDto<List<GetTeacherAssignmentDto>>>
    {
        public string TeacherId { get; set; }
    }
}