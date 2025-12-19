using MediatR;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SchoolManagementSystem.Application.Settings;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses
{
    class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, ResponseDto<List<CourseDto>>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IOptions<CacheSettings> cacheSettings;
        private readonly ILogger<GetAllCoursesHandler> logger;

        public GetAllCoursesHandler(IGenericRepository<Course> repository,
                                    IMapper mapper,
                                    IMemoryCache memoryCache,
                                    IOptions<CacheSettings> cacheSettings,
                                    ILogger<GetAllCoursesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.cacheSettings = cacheSettings;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var coursesKey = CacheKeys.CoursesList;

            TimeSpan cacheExpiration = TimeSpan.FromMinutes(cacheSettings.Value.CoursesCacheExpirationMinutes);

            try
            {
                if (memoryCache.TryGetValue(coursesKey, out List<CourseDto>? courses))
                {
                    logger.LogInformation("Courses retrieved from cache. Total count: {CoursesCount}", courses.Count);
                    return ResponseDto<List<CourseDto>>.Success(courses, "Courses retrieved successfully");
                }

                logger.LogInformation("Courses not found in cache. Fetching from database.");

                courses = await repository
                  .GetAllAsNoTracking()
                  .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);

                if (courses.Count == 0)
                {
                    logger.LogWarning("No courses found in the database. Skipping cache storage.");
                    return ResponseDto<List<CourseDto>>.Error(ErrorCode.NotFound, "No courses found");
                }

                memoryCache.Set(coursesKey, courses, cacheExpiration);

                logger.LogInformation("Courses cached with expiration of {ExpirationMinutes} minutes. Total courses: {CoursesCount}", cacheSettings.Value.CoursesCacheExpirationMinutes, courses.Count);
                return ResponseDto<List<CourseDto>>.Success(courses, "Courses retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving courses.");
                return ResponseDto<List<CourseDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve courses.");
            }
        }
    }
}