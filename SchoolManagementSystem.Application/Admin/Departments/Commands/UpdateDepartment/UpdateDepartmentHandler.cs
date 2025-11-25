using AutoMapper;
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

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment
{
    class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;

        public UpdateDepartmentHandler(IGenericRepository<Department> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = repository.GetByID(request.DepartmentDto.Id);
                if (department == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Department not found");

                mapper.Map(request.DepartmentDto, department);
                repository.Update(department);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Department Updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to update department{ex.Message}");
            }
        }
    }
}
