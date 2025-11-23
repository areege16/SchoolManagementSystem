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

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment
{
    class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;

        public CreateDepartmentHandler(IGenericRepository<Department> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = mapper.Map<Department>(request.DepartmentDto);
                repository.Add(department);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Department Created successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to create department{ex.Message}");
            }

        }
    }
}
