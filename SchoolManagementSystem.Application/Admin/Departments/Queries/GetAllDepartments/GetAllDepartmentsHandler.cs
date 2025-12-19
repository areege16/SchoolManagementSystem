using MediatR;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.Settings;
using SchoolManagementSystem.Application.Common;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetAllDepartments
{
    class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, ResponseDto<List<DepartmentDto>>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IOptions<CacheSettings> cacheSettings;
        private readonly ILogger<GetAllDepartmentsHandler> logger;

        public GetAllDepartmentsHandler(IGenericRepository<Department> repository,
                                        IMapper mapper,
                                        IMemoryCache memoryCache,
                                        IOptions<CacheSettings> cacheSettings,
                                        ILogger<GetAllDepartmentsHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.cacheSettings = cacheSettings;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departmentsKey = CacheKeys.DepartmentsList;

            TimeSpan cacheExpiration = TimeSpan.FromMinutes(cacheSettings.Value.DepartmentsCacheExpirationMinutes);

            try
            {
                if (memoryCache.TryGetValue(departmentsKey, out List<DepartmentDto>? departments))
                {
                    logger.LogInformation("Departments retrieved from cache. Total count: {DepartmentsCount}", departments.Count);
                    return ResponseDto<List<DepartmentDto>>.Success(departments, "Departments retrieved successfully");
                }

                logger.LogInformation("Departments not found in cache. Fetching from database.");

                departments = await repository
                    .GetAllAsNoTracking()
                    .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                if (departments.Count == 0)
                {
                    logger.LogWarning("No departments found in the database. Skipping cache storage.");
                    return ResponseDto<List<DepartmentDto>>.Error(ErrorCode.NotFound, "No department found");
                }

                memoryCache.Set(departmentsKey, departments , cacheExpiration);

                logger.LogInformation("Departments cached with expiration of {ExpirationMinutes} minutes. Total departments: {DepartmentsCount}", cacheSettings.Value.DepartmentsCacheExpirationMinutes, departments.Count);

                return ResponseDto<List<DepartmentDto>>.Success(departments, "Departments retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving departments.");
                return ResponseDto<List<DepartmentDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve departments.");
            }
        }
    }
}