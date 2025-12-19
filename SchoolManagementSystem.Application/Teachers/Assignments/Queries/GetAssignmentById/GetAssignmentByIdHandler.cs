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
    public class GetAssignmentByIdHandler : IRequestHandler<GetAssignmentByIdQuery, ResponseDto<AssignmentDto>>
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAssignmentByIdHandler> logger;

        public GetAssignmentByIdHandler(IGenericRepository<Assignment> repository,
                                        IMapper mapper,
                                        ILogger<GetAssignmentByIdHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<AssignmentDto>> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var assignment = await repository
                         .GetAllAsNoTracking()
                         .Where(d => d.Id == request.AssignmentId && d.CreatedByTeacherId == request.TeacherId)
                         .ProjectTo<AssignmentDto>(mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync(cancellationToken);

                if (assignment == null) //TODO: review assignment message (if teacher not autherize)
                {
                    logger.LogWarning("Assignment with ID {AssignmentId} not found", request.AssignmentId);
                    return ResponseDto<AssignmentDto>.Error(ErrorCode.NotFound, $"Assignment with id {request.AssignmentId} not found ");
                }

                logger.LogInformation("Assignment with ID {AssignmentId} retrieved successfully", request.AssignmentId);
                return ResponseDto<AssignmentDto>.Success(assignment, $"Assignment with id {request.AssignmentId} retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve assignment with ID {AssignmentId}", request.AssignmentId);
                return ResponseDto<AssignmentDto>.Error(ErrorCode.DatabaseError, "Failed to retrieve assignment.");
            }
        }
    }
}