using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common.Pagination;
using SchoolManagementSystem.Application.Common.Responses;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    public class GetStudentEnrolledClassesHandler : IRequestHandler<GetStudentEnrolledClassesQuery, ResponseDto<PagedResult<EnrolledClassDto>>>
    {
        private readonly IGenericRepository<StudentClass> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetStudentEnrolledClassesHandler> logger;

        public GetStudentEnrolledClassesHandler(IGenericRepository<StudentClass> repository,
                                                IMapper mapper,
                                                ILogger<GetStudentEnrolledClassesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<PagedResult<EnrolledClassDto>>> Handle(GetStudentEnrolledClassesQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.StudentId;
            try
            {
                logger.LogInformation("Retrieving all enrolled classes for student {StudentId}", studentId);

                var query = repository
                     .GetAllAsNoTracking()
                     .Where(s => s.StudentId == studentId);

                var totalCount = await query.CountAsync(cancellationToken);

                if (totalCount == 0)
                {
                    logger.LogWarning("No enrolled classes found for student {StudentId}", studentId);
                    return ResponseDto<PagedResult<EnrolledClassDto>>.Error(ErrorCode.NotFound, "No enrolled classes found");
                }

                var classes = await query
                     .OrderBy(sc => sc.EnrollmentDate)
                     .Skip((request.PageNumber - 1) * request.PageSize)
                     .Take(request.PageSize)
                     .ProjectTo<EnrolledClassDto>(mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);

                var result = new PagedResult<EnrolledClassDto>
                {
                    Items = classes,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };

                logger.LogInformation("Successfully retrieved {ItemCount} enrolled classes for student {StudentId} (Page {Page}/{TotalPages}, Total: {TotalCount})", classes.Count, studentId, request.PageNumber, result.TotalPages, totalCount);
                return ResponseDto<PagedResult<EnrolledClassDto>>.Success(result, "Enrolled classes retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve enrolled classes for student {StudentId}", studentId);
                return ResponseDto<PagedResult<EnrolledClassDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve enrolled classes.");
            }
        }
    }
}