using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourse
{
    class CreateCourseHandler : IRequestHandler<CreateCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateCourseHandler> logger;
        private readonly IMemoryCache memoryCache;

        public CreateCourseHandler(IGenericRepository<Course> repository,
                                   IMapper mapper,
                                   ILogger<CreateCourseHandler> logger,
                                   IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to create course with name: {CourseName}", request.AdminId, request.CourseDto.Name);

                var course = mapper.Map<Course>(request.CourseDto);
                repository.Add(course);
                await repository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.CoursesList);

                logger.LogInformation("Admin {AdminId} successfully created course with Id {CourseId} with name: {CourseName}", request.AdminId, course.Id, course.Name);

                return ResponseDto<bool>.Success(true, "Course Added successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to create course. Course name: {CourseName}", request.AdminId, request.CourseDto.Name);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to add course");
            }
        }
    }
}