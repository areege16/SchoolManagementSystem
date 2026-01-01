using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Notification;
using SchoolManagementSystem.Application.Services.NotificationStreamService;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission
{
    public class GradeStudentSubmissionHandler : IRequestHandler<GradeStudentSubmissionCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Submission> submissionRepository;
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IGenericRepository<Notification> notificationRepository;
        private readonly ILogger<GradeStudentSubmissionHandler> logger;
        private readonly INotificationStreamService notificationStreamService;

        public GradeStudentSubmissionHandler(IGenericRepository<Submission> submissionRepository,
                                             IGenericRepository<Assignment> assignmentRepository,
                                             IGenericRepository<Notification> notificationRepository,
                                             ILogger<GradeStudentSubmissionHandler> logger,
                                             INotificationStreamService notificationStreamService)
        {
            this.submissionRepository = submissionRepository;
            this.assignmentRepository = assignmentRepository;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.notificationStreamService = notificationStreamService;
        }
        public async Task<ResponseDto<bool>> Handle(GradeStudentSubmissionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.GradeStudentSubmissionDto;
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Grading submission for Assignment {AssignmentId}, Student {StudentId} by Teacher {TeacherId}. Grade: {Grade}, Remarks: {Remarks}",
                   dto.AssignmentId, dto.StudentId, teacherId, dto.Grade, dto.Remarks);

                var assignmentTeacherId = await assignmentRepository
                  .GetAllAsNoTracking()
                  .Where(a => a.Id == dto.AssignmentId)
                  .Select(a => a.CreatedByTeacherId)
                  .FirstOrDefaultAsync(cancellationToken);

                if (assignmentTeacherId != teacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} is not authorized to grade Assignment {AssignmentId} (Owner: {OwnerTeacherId}).", teacherId, dto.AssignmentId, assignmentTeacherId);
                    return ResponseDto<bool>.Error(ErrorCode.Unauthorized, "You are not authorized to grade this assignment.");
                }

                var submission = await submissionRepository
                 .GetFiltered(s => s.AssignmentId == dto.AssignmentId &&
                                s.StudentId == dto.StudentId, asTracking: true)
                 .FirstOrDefaultAsync(cancellationToken);

                if (submission == null)
                {
                    logger.LogWarning("Submission not found for Assignment {AssignmentId} and Student {StudentId}. Teacher {TeacherId} attempted to grade.", dto.AssignmentId, dto.StudentId, teacherId);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Submission not found");
                }

                var oldGrade = submission.Grade;
                var oldRemarks = submission.Remarks;

                submission.Grade = dto.Grade;
                submission.Remarks = dto.Remarks;
                submission.GradedByTeacherId = request.TeacherId;

                var dbNotification = new Notification
                {
                    Title = "Assignment Graded",
                    Message = $"Your assignment has been graded with a score of {dto.Grade}.",
                    IsRead = false,
                    RecipientId = dto.StudentId,
                    RecipientRole = "Student",
                    CreatedDate = DateTime.UtcNow
                };
                notificationRepository.Add(dbNotification);

                await submissionRepository.SaveChangesAsync(cancellationToken);

                try
                {
                    var notificationDto = new NotificationDto
                    {
                        Id = dbNotification.Id,
                        Title = dbNotification.Title,
                        Message = dbNotification.Message,
                        IsRead = dbNotification.IsRead,
                        CreatedAt = dbNotification.CreatedDate
                    };
                    await notificationStreamService.NotifyUser(dto.StudentId, notificationDto);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to send real-time notification to student {StudentId}. Notification saved in database.", dto.StudentId);
                }

                logger.LogInformation(
                    "Submission graded successfully. Assignment {AssignmentId}, Student {StudentId}. Old Grade: {OldGrade}, New Grade: {NewGrade}. Old Remarks: {OldRemarks}, New Remarks: {NewRemarks}. Graded by Teacher {TeacherId}",
                    dto.AssignmentId, dto.StudentId, oldGrade ?? 0, dto.Grade, oldRemarks ?? "N/A", dto.Remarks, teacherId);

                return ResponseDto<bool>.Success(true, "Submission graded successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to grade submission for Assignment {AssignmentId}, Student {StudentId} by Teacher {TeacherId}. Error: {ErrorMessage}", dto.AssignmentId, dto.StudentId, teacherId, ex.Message);
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to grade Submission");
            }
        }
    }
}