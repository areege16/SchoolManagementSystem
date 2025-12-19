using MediatR;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades
{
    public class GetStudentGradesHandler : IRequestHandler<GetStudentGradesQuery, ResponseDto<List<GetStudentGradeDto>>>
    {
        private readonly IGenericRepository<Submission> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetStudentGradesHandler> logger;

        public GetStudentGradesHandler(IGenericRepository<Submission> repository,
                                       IMapper mapper,
                                       ILogger<GetStudentGradesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<GetStudentGradeDto>>> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.studentId;
            try
            {
                logger.LogInformation("Retrieving grades for student {StudentId}", studentId);

                var grades = await repository
                   .GetAllAsNoTracking()
                   .Where(s => s.StudentId == request.studentId)
                   .ProjectTo<GetStudentGradeDto>(mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

                if (grades.Count == 0)
                {
                    logger.LogWarning("No grades found for student {StudentId}", studentId);

                    return ResponseDto<List<GetStudentGradeDto>>.Error(ErrorCode.NotFound, "No grads found ");
                }
                logger.LogInformation("Successfully retrieved {GradeCount} grades for student {StudentId}", grades.Count, studentId);

                return ResponseDto<List<GetStudentGradeDto>>.Success(grades, "Grades retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve grades for student {StudentId}", studentId);

                return ResponseDto<List<GetStudentGradeDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve grades.");
            }
        }
    }
}