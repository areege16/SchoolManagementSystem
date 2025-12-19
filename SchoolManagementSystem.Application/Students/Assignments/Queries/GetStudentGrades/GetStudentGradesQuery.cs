using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades
{
    public class GetStudentGradesQuery : IRequest<ResponseDto<List<GetStudentGradeDto>>>
    {
        public string studentId { get; set; }
    }
}