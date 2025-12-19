using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateDepartmentHandler> logger;
        private readonly IMemoryCache memoryCache;

        public UpdateDepartmentHandler(IGenericRepository<Department> repository,
                                       IMapper mapper,
                                       ILogger<UpdateDepartmentHandler> logger,
                                       IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to update department with id: {DepartmentId}", request.AdminId, request.DepartmentDto.Id);

                var department = await repository.FindByIdAsync(request.DepartmentDto.Id, cancellationToken);
                if (department == null)
                {
                    logger.LogWarning("Admin {AdminId} tried to update non-existent department {DepartmentId}", request.AdminId, request.DepartmentDto.Id);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Department not found");
                }

                mapper.Map(request.DepartmentDto, department);
                department.UpdatedDate = DateTime.UtcNow;
                await repository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.DepartmentsList);

                logger.LogInformation("Admin {AdminId} successfully updated department {DepartmentId}", request.AdminId, request.DepartmentDto.Id);

                return ResponseDto<bool>.Success(true, "Department Updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to update department {DepartmentId}", request.AdminId, request.DepartmentDto.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to update department{ex.Message}");
            }
        }
    }
}