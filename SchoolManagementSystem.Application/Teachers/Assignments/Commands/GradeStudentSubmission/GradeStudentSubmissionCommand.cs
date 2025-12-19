using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission
{
    public class GradeStudentSubmissionCommand : IRequest<ResponseDto<bool>>
    {
        public GradeStudentSubmissionDto GradeStudentSubmissionDto { get; set; }
        public string TeacherId { get; set; }
    }
}
