using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Department;

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IRequest<ResponseDto<DepartmentDto>>
    {
        public int Id { get; set; }
    }
}