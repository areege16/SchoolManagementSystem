using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment
{
    class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateDepartmentHandler> logger;
        private readonly IMemoryCache memoryCache;

        public CreateDepartmentHandler(IGenericRepository<Department> repository,
                                       IMapper mapper,
                                       ILogger<CreateDepartmentHandler> logger,
                                       IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to create department with name: {DepartmentName}", request.AdminId, request.DepartmentDto.Name);

                var department = mapper.Map<Department>(request.DepartmentDto);
                repository.Add(department);
                await repository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.DepartmentsList);

                logger.LogInformation("Admin {AdminId} successfully created department with Id {DepartmentId} with name: {DepartmentName}", request.AdminId, department.Id, department.Name);

                return ResponseDto<bool>.Success(true, "Department created successfully");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to create department . department name: {DepartmentName}", request.AdminId, request.DepartmentDto.Name);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to create department");
            }
        }
    }
}