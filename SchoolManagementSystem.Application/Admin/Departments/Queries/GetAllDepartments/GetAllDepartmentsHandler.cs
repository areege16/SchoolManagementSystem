using MediatR;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetAllDepartments
{
    class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, ResponseDto<List<DepartmentDto>>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;

        public GetAllDepartmentsHandler(IGenericRepository<Department> repository, IMapper mapper )
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            
            var departments = await repository
                .GetAll()
                .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (departments.Count == 0)
                return ResponseDto<List<DepartmentDto>>.Error(ErrorCode.NotFound, "No department found");

            return ResponseDto<List<DepartmentDto>>.Success(departments, "Departments retrieved successfully");
        }
    }
}
