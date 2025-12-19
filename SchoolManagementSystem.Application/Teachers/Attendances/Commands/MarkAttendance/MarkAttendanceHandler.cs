using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    public class MarkAttendanceHandler : IRequestHandler<MarkAttendanceCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IMapper mapper;
        private readonly ILogger<MarkAttendanceHandler> logger;

        public MarkAttendanceHandler(IGenericRepository<Attendance> attendanceRepository,
                                     IGenericRepository<Class> classRepository,
                                     IGenericRepository<StudentClass> studentClassRepository,
                                     IMapper mapper,
                                     ILogger<MarkAttendanceHandler> logger)
        {
            this.attendanceRepository = attendanceRepository;
            this.classRepository = classRepository;
            this.studentClassRepository = studentClassRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AttendanceDto;
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to mark attendance for Student {StudentId} in Class {ClassId}", teacherId, dto.StudentId, dto.ClassId);

                var classTeacherId = await classRepository
                    .GetAllAsNoTracking()
                    .Where(c => c.Id == dto.ClassId)
                    .Select(c => c.TeacherId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (classTeacherId != teacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} is not authorized to mark attendance for Class {ClassId} (Owner: {OwnerTeacherId}).", teacherId, dto.ClassId, classTeacherId);
                    return ResponseDto<bool>.Error(ErrorCode.Unauthorized, "You are not authorized to mark attendance for this class.");
                }

                var isStudentEnrolled = await studentClassRepository
                    .GetAllAsNoTracking()
                    .AnyAsync(s => s.ClassId == dto.ClassId && s.StudentId == dto.StudentId, cancellationToken);

                if (!isStudentEnrolled)
                {
                    logger.LogWarning("Student {StudentId} is not enrolled in Class {ClassId}. Attendance marking rejected.", dto.StudentId, dto.ClassId);
                    return ResponseDto<bool>.Error(ErrorCode.InvalidInput, "Student is not enrolled in this class.");
                }

                var existing = await attendanceRepository
                   .GetFiltered(a => a.ClassId == dto.ClassId
                                     && a.StudentId == dto.StudentId
                                     && a.Date == DateTime.UtcNow, asTracking: false)
                   .AnyAsync(cancellationToken);

                if (existing)
                {
                    logger.LogWarning("Duplicate attendance attempt blocked for Student {StudentId} in Class {ClassId} on {AttendanceDate}. ", dto.StudentId, dto.ClassId, DateTime.UtcNow);
                    return ResponseDto<bool>.Error(ErrorCode.Conflict, "Attendance already marked for today.");
                }

                var attendance = mapper.Map<Attendance>(request.AttendanceDto);
                attendance.MarkedByTeacherId = teacherId;
                attendance.Date = DateTime.UtcNow;

                attendanceRepository.Add(attendance);
                await attendanceRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Attendance successfully marked by Teacher {TeacherId} for Student {StudentId} in Class {ClassId}", teacherId, dto.StudentId, dto.ClassId);

                return ResponseDto<bool>.Success(true, "Attendance marked successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to mark attendance by Teacher {TeacherId} for Student {StudentId} in Class {ClassId}", teacherId, dto.StudentId, dto.ClassId);

                return ResponseDto<bool>.Error(ErrorCode.OperationFailed, $"Failed to mark attendance.");
            }
        }
    }
}