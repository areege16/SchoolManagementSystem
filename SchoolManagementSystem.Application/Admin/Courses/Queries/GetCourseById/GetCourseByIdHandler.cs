using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById
{
    class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, ResponseDto<CourseDto>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetCourseByIdHandler> logger;

        public GetCourseByIdHandler(IGenericRepository<Course> repository,
                                    IMapper mapper,
                                    ILogger<GetCourseByIdHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await repository
               .GetAllAsNoTracking()
               .Where(c => c.Id == request.Id)
               .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(cancellationToken);

                if (course == null)
                {
                    logger.LogWarning("Course with ID {CourseId} not found", request.Id);
                    return ResponseDto<CourseDto>.Error(ErrorCode.NotFound, $" Course with ID {request.Id} not found");
                }

                logger.LogInformation("Course with ID {CourseId} retrieved successfully", request.Id);
                return ResponseDto<CourseDto>.Success(course, $"Course with id {request.Id} retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve course with ID {CourseId}", request.Id);
                return ResponseDto<CourseDto>.Error(ErrorCode.DatabaseError, "Failed to retrieve course.");
            }
        }
    }
}