using MediatR;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Department;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<ResponseDto<bool>>
    {
        public UpdateDepartmentDto DepartmentDto { get; set; }
        public string AdminId { get; set; }
    }
}
