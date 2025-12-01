using MediatR;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Attendance;
using SchoolManagementSystem.Domain.Enums;
using System.Security.Claims;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentAssignments
{
    public class GetStudentAssignmentsHandler : IRequestHandler<GetStudentAssignmentsQuery, ResponseDto<List<GetStudentAssignmentDto>>>
    {
        private readonly IGenericRepository<Assignment> assignmentRepository;
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IMapper mapper;

        public GetStudentAssignmentsHandler(IGenericRepository<Assignment> assignmentRepository,IGenericRepository<StudentClass> studentClassRepository, IMapper mapper)
        {
            this.assignmentRepository = assignmentRepository;
            this.studentClassRepository = studentClassRepository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<GetStudentAssignmentDto>>> Handle(GetStudentAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var studentClassIds = await studentClassRepository
               .GetAll()
               .Where(sc => sc.StudentId == studentId)
               .Select(sc => sc.ClassId)
               .ToListAsync();

            var assignments =await assignmentRepository
                .GetAll()
                .Where(a => studentClassIds.Contains(a.ClassId))
                .ProjectTo<GetStudentAssignmentDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (assignments.Count == 0)
                return ResponseDto<List<GetStudentAssignmentDto>>.Error(ErrorCode.NotFound, "No assignments found ");

            return ResponseDto<List<GetStudentAssignmentDto>>.Success(assignments, "Assignments retrieved successfully");

        }
    }
}
