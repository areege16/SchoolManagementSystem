using MediatR;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Application.Students.Assignments.Queries.GetStudentGrades
{
    public class GetStudentGradesHandler : IRequestHandler<GetStudentGradesQuery, ResponseDto<List<GetStudentGradeDto>>>
    {
        private readonly IGenericRepository<Submission> repository;
        private readonly IMapper mapper;

        public GetStudentGradesHandler(IGenericRepository<Submission> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<GetStudentGradeDto>>> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var grades = await repository
            .GetAll()
            .Where(s =>s.StudentId==studentId)
            .ProjectTo<GetStudentGradeDto>(mapper.ConfigurationProvider)
            .ToListAsync();

            if (grades.Count == 0)
                return ResponseDto<List<GetStudentGradeDto>>.Error(ErrorCode.NotFound, "No grads found ");

            return ResponseDto<List<GetStudentGradeDto>>.Success(grades, "Grades retrieved successfully");
        }
    }
}
