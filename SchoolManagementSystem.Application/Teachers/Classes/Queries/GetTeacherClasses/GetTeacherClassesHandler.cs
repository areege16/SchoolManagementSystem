using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper.QueryableExtensions;
using SchoolManagementSystem.Application.Common.Pagination;
using SchoolManagementSystem.Application.Common.Responses;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses
{
    public class GetTeacherClassesHandler : IRequestHandler<GetTeacherClassesQuery, ResponseDto<PagedResult<ClassDto>>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetTeacherClassesHandler> logger;

        public GetTeacherClassesHandler(IGenericRepository<Class> repository,
                                    IMapper mapper,
                                    ILogger<GetTeacherClassesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<PagedResult<ClassDto>>> Handle(GetTeacherClassesQuery request, CancellationToken cancellationToken)
        {
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Retrieving all classes for teacher {TeacherId}.", teacherId);

                var query = repository
                .GetAllAsNoTracking()
                .Where(c => c.TeacherId == teacherId);

                var totalCount = await query.CountAsync(cancellationToken);

                if (totalCount == 0)
                {
                    logger.LogWarning("No classes found for Teacher {TeacherId}.", teacherId);
                    return ResponseDto<PagedResult<ClassDto>>.Error(ErrorCode.NotFound, "No classes found for this teacher.");
                }

                var classes = await query
                    .OrderBy(c => c.Id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectTo<ClassDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var result = new PagedResult<ClassDto>
                {
                    Items = classes,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };

                logger.LogInformation("Successfully retrieved {ItemCount} classes for teacher {TeacherId}. (Page {Page}/{TotalPages}, Total: {TotalCount}).", classes.Count, teacherId, request.PageNumber, result.TotalPages, totalCount);
                return ResponseDto<PagedResult<ClassDto>>.Success(result, "Classes retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve enrolled classes for teacher {TeacherId}", teacherId);
                return ResponseDto<PagedResult<ClassDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve classes.");
            }
        }
    }
}