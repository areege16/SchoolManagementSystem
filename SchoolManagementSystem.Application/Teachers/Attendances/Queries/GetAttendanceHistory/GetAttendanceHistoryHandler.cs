using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;
using Microsoft.Extensions.Logging;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Queries.GetAttendanceHistory
{
    public class GetAttendanceHistoryHandler : IRequestHandler<GetAttendanceHistoryCommand, ResponseDto<List<AttendanceHistoryDto>>>//TODO : Change response to be grouped by Date instead of flat list.
    {
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Class> classRepository;
        private readonly ILogger<GetAttendanceHistoryHandler> logger;

        public GetAttendanceHistoryHandler(IGenericRepository<Attendance> attendanceRepository,
                                           IMapper mapper,
                                           IGenericRepository<Class> classRepository,
                                           ILogger<GetAttendanceHistoryHandler> logger)
        {
            this.attendanceRepository = attendanceRepository;
            this.mapper = mapper;
            this.classRepository = classRepository;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<AttendanceHistoryDto>>> Handle(GetAttendanceHistoryCommand request, CancellationToken cancellationToken)
        {
            var classId = request.ClassId;
            var teacherId = request.TeacherId;

            try
            {
                logger.LogInformation("Teacher {TeacherId} requesting attendance history for Class {ClassId}", teacherId, classId);

                var classTeacherId = await classRepository
                    .GetAllAsNoTracking()
                    .Where(c => c.Id == classId)
                    .Select(c => c.TeacherId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (classTeacherId != request.TeacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} is not authorized to access attendance for Class {ClassId} (Owner: {OwnerTeacherId}).", teacherId, classId, classTeacherId);
                    return ResponseDto<List<AttendanceHistoryDto>>.Error(ErrorCode.Unauthorized, "You are not authorized to view attendance for this class.");
                }

                var attendanceHistory = await attendanceRepository
                    .GetFiltered(i => i.ClassId == request.ClassId, asTracking: false)
                    .ProjectTo<AttendanceHistoryDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                if (attendanceHistory.Count == 0)
                {
                    logger.LogInformation("No attendance records found for Class {ClassId}.", classId);
                    return ResponseDto<List<AttendanceHistoryDto>>.Error(ErrorCode.NotFound, $"No attendance history with id {request.ClassId} found");
                }

                logger.LogInformation("Retrieved {AttendanceCount} attendance records for Class {ClassId}.", attendanceHistory.Count, classId);

                return ResponseDto<List<AttendanceHistoryDto>>.Success(attendanceHistory, $"Attendance history with id {request.ClassId} retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve attendance history for Class {ClassId} by Teacher {TeacherId}.", classId, teacherId);
                return ResponseDto<List<AttendanceHistoryDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve attendance history.");
            }
        }
    }
}