using MediatR;
using SchoolManagementSystem.Application.Common.Pagination;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    public class GetStudentEnrolledClassesQuery : IRequest<ResponseDto<PagedResult<EnrolledClassDto>>>
    {
        public string StudentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}