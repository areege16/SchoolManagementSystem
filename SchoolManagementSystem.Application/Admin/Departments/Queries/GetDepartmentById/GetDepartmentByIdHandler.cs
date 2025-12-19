using MediatR;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById
{
    class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, ResponseDto<DepartmentDto>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetDepartmentByIdHandler> logger;

        public GetDepartmentByIdHandler(IGenericRepository<Department> repository,
                                        IMapper mapper,
                                        ILogger<GetDepartmentByIdHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await repository
               .GetAllAsNoTracking()
               .Where(d => d.Id == request.Id)
               .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(cancellationToken);

                if (department == null)
                {
                    logger.LogWarning("Department with ID {DepartmentId} not found", request.Id);
                    return ResponseDto<DepartmentDto>.Error(ErrorCode.NotFound, $"No department with id {request.Id} found");
                }
                logger.LogInformation("Department with ID {DepartmentId} retrieved successfully", request.Id);

                return ResponseDto<DepartmentDto>.Success(department, $"Department with id {request.Id} retrieved successfully");
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Failed to retrieve department with ID {DepartmentId}", request.Id);
                return ResponseDto<DepartmentDto>.Error(ErrorCode.DatabaseError, "Failed to retrieve department.");
            }
        }
    }
}