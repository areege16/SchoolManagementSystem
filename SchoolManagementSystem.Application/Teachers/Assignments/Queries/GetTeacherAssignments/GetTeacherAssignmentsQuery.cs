using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetTeacherAssignments
{
    public class GetTeacherAssignmentsQuery : IRequest<ResponseDto<List<GetTeacherAssignmentDto>>>
    {
        public string TeacherId { get; set; }
    }
}