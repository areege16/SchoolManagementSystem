using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Assignment;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.GradeStudentSubmission
{
    public class GradeStudentSubmissionHandler : IRequestHandler<GradeStudentSubmissionCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Submission> repository;

        public GradeStudentSubmissionHandler(IGenericRepository<Submission> repository)
        {
            this.repository = repository;
        }
        public async Task<ResponseDto<bool>> Handle(GradeStudentSubmissionCommand request, CancellationToken cancellationToken)
        {         
            try
            {
                var teacherId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var submission = await repository.GetAll()
                 .Where(s => s.AssignmentId == request.GradeStudentSubmissionDto.AssignmentId &&
                                s.StudentId == request.GradeStudentSubmissionDto.StudentId)
                 .FirstOrDefaultAsync();

                if (submission == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Submission not found");

                submission.Grade = request.GradeStudentSubmissionDto.Grade;
                submission.Remarks = request.GradeStudentSubmissionDto.Remarks;
                submission.GradedByTeacherId = teacherId;

                repository.Update(submission);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Submission graded successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to grade Submission: {ex.Message}");
            }
        }
    }
}