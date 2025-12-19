using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentAssignments
{
    public class GetStudentAssignmentsHandler : IRequestHandler<GetStudentAssignmentsQuery, ResponseDto<List<GetStudentAssignmentDto>>>
    {
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetStudentAssignmentsHandler> logger;

        public GetStudentAssignmentsHandler(IGenericRepository<Assignment> assignmentRepository,
                                            IMapper mapper,
                                            ILogger<GetStudentAssignmentsHandler> logger)
        {
            this.assignmentRepository = assignmentRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<GetStudentAssignmentDto>>> Handle(GetStudentAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.StudentId;
            try
            {
                logger.LogInformation("Retrieving assignments for Student {StudentId}", studentId);

                var assignments = await assignmentRepository
                      .GetAllAsNoTracking()
                      .Where(a => a.Class.StudentClasses.Any(sc => sc.StudentId == request.StudentId))
                      .OrderBy(a => a.DueDate)
                      .ProjectTo<GetStudentAssignmentDto>(mapper.ConfigurationProvider)
                      .ToListAsync(cancellationToken);

                if (assignments.Count == 0)
                {
                    logger.LogInformation("No assignments found for Student {StudentId}", studentId);
                    return ResponseDto<List<GetStudentAssignmentDto>>.Error(ErrorCode.NotFound, "No assignments found. ");
                }

                logger.LogInformation("Successfully retrieved {AssignmentCount} assignments for Student {StudentId}", assignments.Count, studentId);

                return ResponseDto<List<GetStudentAssignmentDto>>.Success(assignments, "Assignments retrieved successfully. ");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve assignments for Student {StudentId}", studentId);
                return ResponseDto<List<GetStudentAssignmentDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve assignments.");
            }
        }
    }
}