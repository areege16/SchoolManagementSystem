using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
    class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> repository;

        public DeleteDepartmentHandler(IGenericRepository<Department> repository)
        {
            this.repository = repository;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                repository.Remove(request.Id);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Department deleted successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to create department{ex.Message}");
            }

        }
    }
}
