using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.DeleteClass
{
    public class DeleteClassHandler : IRequestHandler<DeleteClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IGenericRepository<Submission> submissionRepository;
        private readonly ILogger<DeleteClassHandler> logger;

        public DeleteClassHandler(IGenericRepository<Class> classRepository,
                                  IGenericRepository<Assignment> assignmentRepository,
                                  IGenericRepository<Attendance> attendanceRepository,
                                  IGenericRepository<StudentClass> studentClassRepository,
                                  IGenericRepository<Submission> submissionRepository,
                                  ILogger<DeleteClassHandler> logger)
        {
            this.classRepository = classRepository;
            this.assignmentRepository = assignmentRepository;
            this.attendanceRepository = attendanceRepository;
            this.studentClassRepository = studentClassRepository;
            this.submissionRepository = submissionRepository;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to delete class with id: {ClassId}", teacherId, request.Id);

                var cls = await classRepository.FindByIdAsync(request.Id, cancellationToken);
                if (cls == null)
                {
                    logger.LogWarning("Class  with id {ClassId} not found", request.Id);

                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Class with id: {request.Id} not found ");
                }

                if (cls.TeacherId != teacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} tried to delete class {ClassId} owned by {OwnerTeacherId}", teacherId, request.Id, cls.TeacherId);

                    return ResponseDto<bool>.Error(ErrorCode.Unauthorized, "You cannot delete a class that does not belong to you.");
                }

                var assignments = await assignmentRepository
                    .GetFiltered(x => x.ClassId == request.Id, asTracking: true)
                    .ToListAsync(cancellationToken);

                var submissions = await submissionRepository
                    .GetFiltered(s => s.Assignment.ClassId == request.Id, asTracking: true)
                    .ToListAsync(cancellationToken);

                var attendances = await attendanceRepository
                    .GetFiltered(a => a.ClassId == request.Id, asTracking: true)
                    .ToListAsync(cancellationToken);

                var studentClasses = await studentClassRepository
                    .GetFiltered(sc => sc.ClassId == request.Id, asTracking: true)
                    .ToListAsync(cancellationToken);

                submissionRepository.RemoveRange(submissions);
                assignmentRepository.RemoveRange(assignments);
                attendanceRepository.RemoveRange(attendances);
                studentClassRepository.RemoveRange(studentClasses);

                cls.IsActive = false;
                await classRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Teacher {TeacherId} successfully deleted class {ClassId}: " +
                                      "{SubmissionsCount} submissions, {assignmentCount} assignments, " +
                                      "{AttendancesCount} attendances, {studentClassCount} student-class relations",
                                      teacherId,
                                      request.Id,
                                      submissions.Count,
                                      assignments.Count,
                                      attendances.Count,
                                      studentClasses.Count);

                return ResponseDto<bool>.Success(true, $"Class with id {request.Id} deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Teacher {TeacherId} failed to delete class. Class id: {ClassId}", teacherId, request.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to delete class");
            }
        }
    }
}