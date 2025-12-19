using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse
{
    class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateCourseHandler> logger;
        private readonly IMemoryCache memoryCache;

        public UpdateCourseHandler(IGenericRepository<Course> repository,
                                   IMapper mapper,
                                   ILogger<UpdateCourseHandler> logger,
                                   IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to update course with id: {CourseId}", request.AdminId, request.CourseDto.Id);

                var course = await repository.FindByIdAsync(request.CourseDto.Id, cancellationToken);

                if (course == null)
                {
                    logger.LogWarning("Admin {AdminId} tried to update non-existent course {CourseId}", request.AdminId, request.CourseDto.Id);

                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Course with id {request.CourseDto.Id} not found.");
                }

                mapper.Map(request.CourseDto, course);
                course.UpdatedDate = DateTime.UtcNow;

                await repository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.CoursesList);

                logger.LogInformation("Admin {AdminId} successfully updated course {CourseId}.", request.AdminId, request.CourseDto.Id);

                return ResponseDto<bool>.Success(true, "Course information updated successfully. ");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to update course {CourseId}", request.AdminId, request.CourseDto.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to update course");
            }
        }
    }
}