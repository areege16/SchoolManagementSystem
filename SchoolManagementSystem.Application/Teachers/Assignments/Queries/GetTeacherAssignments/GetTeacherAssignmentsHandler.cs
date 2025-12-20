using MediatR;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetTeacherAssignments
{
    public class GetTeacherAssignmentsHandler : IRequestHandler<GetTeacherAssignmentsQuery, ResponseDto<List<GetTeacherAssignmentDto>>>
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly ILogger<GetTeacherAssignmentsHandler> logger;
        private readonly IMapper mapper;

        public GetTeacherAssignmentsHandler(IGenericRepository<Assignment> repository,
                                            ILogger<GetTeacherAssignmentsHandler> logger,
                                            IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<GetTeacherAssignmentDto>>> Handle(GetTeacherAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Retrieving assignments for teacher {TeacherId}", teacherId);

                var assignments = await repository
               .GetAllAsNoTracking()
               .Where(a => a.CreatedByTeacherId == teacherId)
               .OrderBy(a => a.DueDate)
               .ProjectTo<GetTeacherAssignmentDto>(mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);

                if (assignments.Count == 0)
                {
                    logger.LogInformation("No assignments found for teacher {TeacherId}", teacherId);
                    return ResponseDto<List<GetTeacherAssignmentDto>>.Error(ErrorCode.NotFound, "No assignments found.");
                }
                logger.LogInformation("Successfully retrieved {AssignmentCount} assignments for teacher {TeacherId}", assignments.Count, teacherId);

                return ResponseDto<List<GetTeacherAssignmentDto>>.Success(assignments, "Assignments retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve assignments for teacher {TeacherId}", teacherId);
                return ResponseDto<List<GetTeacherAssignmentDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve assignments.");
            }
        }
    }
}