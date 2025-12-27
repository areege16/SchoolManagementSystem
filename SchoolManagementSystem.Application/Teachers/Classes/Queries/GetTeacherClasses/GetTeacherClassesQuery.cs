using MediatR;
using SchoolManagementSystem.Application.Common.Pagination;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Class;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses
{
    public class GetTeacherClassesQuery : IRequest<ResponseDto<PagedResult<ClassDto>>>
    {
        public string TeacherId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}