using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse
{
    class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> courseRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IGenericRepository<Submission> submissionRepository;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<DeleteCourseHandler> logger;

        public DeleteCourseHandler(IGenericRepository<Course> courseRepository,
                                   IGenericRepository<Class> classRepository,
                                   IGenericRepository<Assignment> assignmentRepository,
                                   IGenericRepository<Attendance> attendanceRepository,
                                   IGenericRepository<StudentClass> studentClassRepository,
                                   IGenericRepository<Submission> submissionRepository,
                                   IMemoryCache memoryCache,
                                   ILogger<DeleteCourseHandler> logger)
        {
            this.courseRepository = courseRepository;
            this.classRepository = classRepository;
            this.assignmentRepository = assignmentRepository;
            this.attendanceRepository = attendanceRepository;
            this.studentClassRepository = studentClassRepository;
            this.submissionRepository = submissionRepository;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var adminId = request.AdminId;
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to delete course with id: {CourseId}", adminId, request.Id);

                var course = await courseRepository.FindByIdAsync(request.Id, cancellationToken);
                if (course == null)
                {
                    logger.LogWarning("Course with id {CourseId} not found", request.Id);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Course with id {request.Id} not found");
                }

                var submissions = await submissionRepository
                    .GetFiltered(s => s.Assignment.Class.CourseId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var assignments = await assignmentRepository
                    .GetFiltered(x => x.Class.CourseId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var attendances = await attendanceRepository
                    .GetFiltered(a => a.Class.CourseId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var studentClasses = await studentClassRepository
                    .GetFiltered(sc => sc.Class.CourseId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var classes = await classRepository
                    .GetFiltered(c => c.CourseId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                submissionRepository.RemoveRange(submissions);
                assignmentRepository.RemoveRange(assignments);
                attendanceRepository.RemoveRange(attendances);
                studentClassRepository.RemoveRange(studentClasses);
                classRepository.RemoveRange(classes);

                courseRepository.Remove(course);

                await courseRepository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.CoursesList);

                logger.LogInformation("Admin {AdminId} successfully deleted course with id {CourseId}: " +
                                      "{ClassesCount} classes, {SubmissionsCount} submissions, {AssignmentsCount} assignments, " +
                                      "{AttendancesCount} attendances, {StudentClassesCount} student-class relations",
                                      request.AdminId,
                                      request.Id,
                                      classes.Count,
                                      submissions.Count,
                                      assignments.Count,
                                      attendances.Count,
                                      studentClasses.Count);

                return ResponseDto<bool>.Success(true, $"Course with id {request.Id} deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to delete course. Course id: {CourseId}", adminId, request.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to delete course");
            }
        }
    }
}