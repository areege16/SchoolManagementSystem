using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById
{
    public class GetAssignmentDetailsHandler : IRequestHandler<GetAssignmentDetailsQuery, ResponseDto<AssignmentDetailsDto>> // TODO: Include submission details when fetching assignment by ID
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAssignmentDetailsHandler> logger;

        public GetAssignmentDetailsHandler(IGenericRepository<Assignment> repository,
                                        IMapper mapper,
                                        ILogger<GetAssignmentDetailsHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<AssignmentDetailsDto>> Handle(GetAssignmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var assignmentId = request.AssignmentId;
            try
            {
                var assignment = await repository
                         .GetAllAsNoTracking()
                         .Where(d => d.Id == assignmentId && d.CreatedByTeacherId == request.TeacherId)
                         .ProjectTo<AssignmentDetailsDto>(mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync(cancellationToken);

                if (assignment == null)
                {
                    logger.LogWarning("Assignment with ID {AssignmentId} not found or not authorized for teacher {TeacherId}", assignmentId, request.TeacherId);
                    return ResponseDto<AssignmentDetailsDto>.Error(ErrorCode.NotFound, $"Assignment with id {assignmentId} not found ");
                }

                logger.LogInformation("Assignment with ID {AssignmentId} retrieved successfully", assignmentId);
                return ResponseDto<AssignmentDetailsDto>.Success(assignment, $"Assignment with id {assignmentId} retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve assignment with ID {AssignmentId}", assignmentId);
                return ResponseDto<AssignmentDetailsDto>.Error(ErrorCode.DatabaseError, "Failed to retrieve assignment.");
            }
        }
    }
}