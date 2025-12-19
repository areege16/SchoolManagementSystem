using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment
{
    public class SubmitAssignmentHandler : IRequestHandler<SubmitAssignmentCommand, ResponseDto<bool>> //TODO: Extract file saving logic into a dedicated IFileStorageService
    {
        private readonly IGenericRepository<Submission> submissionRepository;
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<SubmitAssignmentHandler> logger;

        private const long MaxFileSize = 10 * 1024 * 1024;
        public SubmitAssignmentHandler(IGenericRepository<Submission> submissionRepository,
                                       IGenericRepository<Assignment> assignmentRepository,
                                       IWebHostEnvironment env,
                                       ILogger<SubmitAssignmentHandler> logger)
        {
            this.submissionRepository = submissionRepository;
            this.assignmentRepository = assignmentRepository;
            this.env = env;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(SubmitAssignmentCommand request, CancellationToken cancellationToken)
        {
            string? filePath = null;
            try
            {
                var dto = request.SubmitAssignmentDto;
                var assignmentExists = await assignmentRepository
                    .GetAllAsNoTracking()
                    .Where(a => a.Id == dto.AssignmentId && a.Class.StudentClasses.Any(sc => sc.StudentId == request.StudentId))
                    .AnyAsync(cancellationToken);
                if (!assignmentExists)
                {
                    logger.LogWarning("Assignment not found or student not enrolled. AssignmentId: {AssignmentId}, StudentId: {StudentId}", dto.AssignmentId, request.StudentId);

                    return ResponseDto<bool>.Error(ErrorCode.Forbidden, "Assignment not found or access denied.");
                }

                var hasSubmitted = await submissionRepository
                      .GetFiltered(s => s.AssignmentId == dto.AssignmentId && s.StudentId == request.StudentId, asTracking: false)
                      .AnyAsync(cancellationToken);
                if (hasSubmitted)
                {
                    logger.LogInformation("Duplicate submission attempt. AssignmentId: {AssignmentId}, StudentId: {StudentId}", dto.AssignmentId, request.StudentId);
                    return ResponseDto<bool>.Error(ErrorCode.AlreadyExists, "You have already submitted this assignment.");
                }

                var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" }; //TODO: move to appsettings
                var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    logger.LogWarning("Invalid file type. StudentId: {StudentId}, FileName: {FileName}", request.StudentId, dto.File.FileName);
                    return ResponseDto<bool>.Error(ErrorCode.InvalidInput, "File type not allowed.");
                }

                if (dto.File.Length > MaxFileSize)
                {
                    logger.LogWarning("File size exceeds limit. StudentId: {StudentId}, FileName: {FileName}, Size: {FileSize}", request.StudentId, dto.File.FileName, dto.File.Length);
                    return ResponseDto<bool>.Error(ErrorCode.InvalidInput, "File size exceeds 10MB.");
                }

                var uploadsPath = Path.Combine(env.WebRootPath, "uploads", "submissions");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var safeFileName = $"{request.StudentId}_{dto.AssignmentId}_{Guid.NewGuid()}{extension}";
                filePath = Path.Combine(uploadsPath, safeFileName);

                await using var stream = File.Create(filePath);
                await dto.File.CopyToAsync(stream, cancellationToken);

                var submission = new Submission
                {
                    AssignmentId = dto.AssignmentId,
                    SubmittedDate = DateTime.UtcNow,
                    FileUrl = $"/uploads/submissions/{safeFileName}"
                };

                submissionRepository.Add(submission);
                await submissionRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Assignment submitted successfully. AssignmentId: {AssignmentId}, StudentId: {StudentId}", dto.AssignmentId, request.StudentId);

                return ResponseDto<bool>.Success(true, "Submitted successfully");
            }
            catch (Exception ex)
            {
                if (filePath != null && File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                        logger.LogInformation("Orphaned file deleted: {FilePath}", filePath);
                    }
                    catch (Exception deleteEx)
                    {
                        logger.LogWarning(deleteEx, "Failed to delete orphaned file: {FilePath}", filePath);
                    }
                }

                logger.LogError(ex, "Error submitting assignment for student {StudentId}", request.StudentId);
                return ResponseDto<bool>.Error(ErrorCode.InternalServerError, "Failed to submit assignment.");
            }
        }
    }
}