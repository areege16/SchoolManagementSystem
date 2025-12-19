using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Department;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<ResponseDto<bool>>
    {
        public CreateDepartmentDto DepartmentDto { get; set; }
        public string AdminId { get; set; }
    }
}