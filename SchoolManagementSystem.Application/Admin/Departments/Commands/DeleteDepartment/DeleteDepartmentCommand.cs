using MediatR;
using SchoolManagementSystem.Application.Common.Responses;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest<ResponseDto<bool>>
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
    }
}