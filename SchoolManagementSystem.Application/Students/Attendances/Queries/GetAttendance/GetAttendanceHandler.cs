using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Attendance.Student;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance
{
    public class GetAttendanceHandler : IRequestHandler<GetAttendanceQuery, ResponseDto<List<GetStudentAttendanceDto>>>
    {
        private readonly IGenericRepository<Attendance> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAttendanceHandler> logger;

        public GetAttendanceHandler(IGenericRepository<Attendance> repository,
                                    IMapper mapper,
                                    ILogger<GetAttendanceHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<GetStudentAttendanceDto>>> Handle(GetAttendanceQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.StudentId;
            try
            {
                logger.LogInformation("Retrieving attendance records for student {StudentId}", studentId);

                var attendances = await repository
                 .GetAllAsNoTracking()
                 .Where(s => s.StudentId == request.StudentId)
                 .ProjectTo<GetStudentAttendanceDto>(mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

                if (attendances.Count == 0)
                {
                    logger.LogWarning("No attendance records found for student {StudentId}", studentId);

                    return ResponseDto<List<GetStudentAttendanceDto>>.Error(ErrorCode.NotFound, "No attendance records found");
                }
                logger.LogInformation("Successfully retrieved {AttendanceCount} attendance records for student {StudentId}", attendances.Count, studentId);

                return ResponseDto<List<GetStudentAttendanceDto>>.Success(attendances, "Attendance records retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve attendance records for student {StudentId}", studentId);
                return ResponseDto<List<GetStudentAttendanceDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve attendance records.");
            }
        }
    }
}