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
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById
{
    class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, ResponseDto<DepartmentDto>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;

        public GetDepartmentByIdHandler(IGenericRepository<Department> repository , IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await repository.GetAll()
                .Where(d => d.Id == request.Id)
                .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return ResponseDto<DepartmentDto>.Success(department, "Departments retrieved successfully");
        }
    }
}
