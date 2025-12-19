using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    class GetEnrolledClassesHandler : IRequestHandler<GetEnrolledClassesCommand, ResponseDto<List<EnrolledClassDto>>>
    {
        private readonly IGenericRepository<StudentClass> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetEnrolledClassesHandler> logger;

        public GetEnrolledClassesHandler(IGenericRepository<StudentClass> repository,
                                         IMapper mapper,
                                         ILogger<GetEnrolledClassesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<EnrolledClassDto>>> Handle(GetEnrolledClassesCommand request, CancellationToken cancellationToken)
        {
            var studentId = request.StudentId;
            try
            {
                logger.LogInformation("Retrieving enrolled classes for student {StudentId}", studentId);

                var classes = await repository
               .GetAllAsNoTracking()
               .Where(s => s.StudentId == request.StudentId)
               .ProjectTo<EnrolledClassDto>(mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);

                if (classes.Count == 0)
                {
                    logger.LogWarning("No enrolled classes found for student {StudentId}", studentId);
                    return ResponseDto<List<EnrolledClassDto>>.Error(ErrorCode.NotFound, "No enrolled classes found");
                }

                logger.LogInformation("Successfully retrieved {ClassCount} enrolled classes for student {StudentId}", classes.Count, studentId);

                return ResponseDto<List<EnrolledClassDto>>.Success(classes, "Enrolled class retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve enrolled classes for student {StudentId}", studentId);
                return ResponseDto<List<EnrolledClassDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve enrolled classes.");
            }
        }
    }
}