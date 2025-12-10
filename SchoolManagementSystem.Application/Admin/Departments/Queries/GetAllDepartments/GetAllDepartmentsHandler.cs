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

namespace SchoolManagementSystem.Application.Admin.Departments.Queries.GetAllDepartments
{
    class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, ResponseDto<List<DepartmentDto>>>
    {
        private readonly IGenericRepository<Department> repository;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IOptions<CacheSettings> cacheSettings;

        public GetAllDepartmentsHandler(IGenericRepository<Department> repository,
                                        IMapper mapper,
                                        IMemoryCache memoryCache,
                                        IOptions<CacheSettings> cacheSettings)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.cacheSettings = cacheSettings;
        }
        public async Task<ResponseDto<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departmentsKey = "Departments";
            TimeSpan cacheExpiration = TimeSpan.FromMinutes(cacheSettings.Value.DepartmentsCacheExpirationMinutes);

            if (!memoryCache.TryGetValue(departmentsKey, out List<DepartmentDto>? departments))
            {
                departments = await repository
                    .GetAll()
                    .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                memoryCache.Set(departmentsKey, departments ?? new List<DepartmentDto>(), cacheExpiration);
            }
            if (departments.Count == 0)
                return ResponseDto<List<DepartmentDto>>.Error(ErrorCode.NotFound, "No department found");

            return ResponseDto<List<DepartmentDto>>.Success(departments, "Departments retrieved successfully");
        }
    }
}
