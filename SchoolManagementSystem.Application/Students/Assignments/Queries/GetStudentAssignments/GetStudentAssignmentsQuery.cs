using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentAssignments
{
    public class GetStudentAssignmentsQuery : IRequest<ResponseDto<List<GetStudentAssignmentDto>>>
    {
        public string StudentId { get; set; }
    }
}