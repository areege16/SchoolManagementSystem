using MediatR;
using Microsoft.AspNetCore.Hosting;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Students.Assignments.Commands.SubmitAssignment
{
    public class SubmitAssignmentHandler : IRequestHandler<SubmitAssignmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Submission> repository;
        private readonly IWebHostEnvironment env;

        public SubmitAssignmentHandler(IGenericRepository<Submission> repository, IWebHostEnvironment env)
        {
            this.repository = repository;
            this.env = env;
        }
        public async Task<ResponseDto<bool>> Handle(SubmitAssignmentCommand request, CancellationToken cancellationToken)
        {
            var studentId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var file = request.SubmitAssignmentDto.File;

            var uploadsPath = Path.Combine(env.WebRootPath, "uploads", "submissions");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var submission = new Submission
            {
                AssignmentId = request.SubmitAssignmentDto.AssignmentId,
                SubmittedDate = DateTime.UtcNow,
                StudentId = studentId,
                FileUrl = $"/uploads/submissions/{fileName}"
            };

            repository.Add(submission);
            await repository.SaveChangesAsync();
            return ResponseDto<bool>.Success(true, "Submitted successfully");
        }
    }
}
