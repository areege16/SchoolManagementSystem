using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
    class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> departmentRepository;
        private readonly IGenericRepository<Course> courseRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IGenericRepository<Submission> submissionRepository;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<DeleteDepartmentHandler> logger;

        public DeleteDepartmentHandler(IGenericRepository<Department> departmentRepository,
                                       IGenericRepository<Course> courseRepository,
                                       IGenericRepository<Class> classRepository,
                                       IGenericRepository<Assignment> assignmentRepository,
                                       IGenericRepository<Attendance> attendanceRepository,
                                       IGenericRepository<StudentClass> studentClassRepository,
                                       IGenericRepository<Submission> submissionRepository,
                                       IMemoryCache memoryCache,
                                       ILogger<DeleteDepartmentHandler> logger)
        {
            this.departmentRepository = departmentRepository;
            this.courseRepository = courseRepository;
            this.classRepository = classRepository;
            this.assignmentRepository = assignmentRepository;
            this.attendanceRepository = attendanceRepository;
            this.studentClassRepository = studentClassRepository;
            this.submissionRepository = submissionRepository;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var adminId = request.AdminId;
            try
            {
                logger.LogInformation("Admin {AdminId} attempting to delete department with id: {DepartmentId}", adminId, request.Id);

                var department = await departmentRepository.FindByIdAsync(request.Id, cancellationToken);
                if (department == null)
                {
                    logger.LogWarning("Department with id {DepartmentId} not found", request.Id);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Department with id {request.Id} not found");
                }

                var submissions = await submissionRepository
                  .GetFiltered(s => s.Assignment.Class.Course.DepartmentId == request.Id, asTracking: true)
                  .IgnoreQueryFilters()
                  .ToListAsync(cancellationToken);

                var assignments = await assignmentRepository
                    .GetFiltered(x => x.Class.Course.DepartmentId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var attendances = await attendanceRepository
                    .GetFiltered(a => a.Class.Course.DepartmentId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var studentClasses = await studentClassRepository
                    .GetFiltered(sc => sc.Class.Course.DepartmentId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var classes = await classRepository
                    .GetFiltered(c => c.Course.DepartmentId == request.Id, asTracking: true)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);

                var courses = await courseRepository
                     .GetFiltered(c => c.DepartmentId == request.Id, asTracking: true)
                     .IgnoreQueryFilters()
                     .ToListAsync(cancellationToken);

                submissionRepository.RemoveRange(submissions);
                assignmentRepository.RemoveRange(assignments);
                attendanceRepository.RemoveRange(attendances);
                studentClassRepository.RemoveRange(studentClasses);
                classRepository.RemoveRange(classes);
                courseRepository.RemoveRange(courses);

                departmentRepository.Remove(department);
                await departmentRepository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.DepartmentsList);

                logger.LogInformation("Admin {AdminId} successfully deleted department with id {DepartmentId}: " +
                                      "{SubmissionsCount} submissions, {AssignmentsCount} assignments, " +
                                      "{AttendancesCount} attendances, {StudentClassesCount} student-class relations, " +
                                      "{ClassesCount} classes, {CoursesCount} courses",
                                      request.AdminId,
                                      request.Id,
                                      submissions.Count,
                                      assignments.Count,
                                      attendances.Count,
                                      studentClasses.Count,
                                      classes.Count,
                                      courses.Count);

                return ResponseDto<bool>.Success(true, $"Department with id {request.Id} deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Admin {AdminId} failed to delete Department. Department id: {DepartmentId}", adminId, request.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to delete department");
            }
        }
    }
}