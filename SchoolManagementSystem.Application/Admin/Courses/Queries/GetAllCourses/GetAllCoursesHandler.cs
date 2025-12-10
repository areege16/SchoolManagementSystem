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

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses
{
    class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, ResponseDto<List<CourseDto>>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IOptions<CacheSettings> cacheSettings;

        public GetAllCoursesHandler(IGenericRepository<Course> repository,
                                    IMapper mapper,
                                    IMemoryCache memoryCache,
                                    IOptions<CacheSettings> cacheSettings)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.cacheSettings = cacheSettings;
        }
        public async Task<ResponseDto<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var coursesKey = "Courses";
            TimeSpan cacheExpiration = TimeSpan.FromMinutes(cacheSettings.Value.CoursesCacheExpirationMinutes);

            if (!memoryCache.TryGetValue(coursesKey, out List<CourseDto>? courses))
            {
                courses = await repository
                .GetAll()
                .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

                memoryCache.Set(coursesKey, courses ?? new List<CourseDto>(), cacheExpiration);
            }

            if (courses.Count == 0)
                return ResponseDto<List<CourseDto>>.Error(ErrorCode.NotFound, "No courses found");

            return ResponseDto<List<CourseDto>>.Success(courses, "Courses retrieved successfully");
        }
    }
}
