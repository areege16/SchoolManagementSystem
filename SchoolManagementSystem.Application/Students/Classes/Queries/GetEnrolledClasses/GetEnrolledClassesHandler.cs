using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SchoolManagementSystem.Application.DTOs.Course;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Enums;
using System.Security.Claims;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    class GetEnrolledClassesHandler : IRequestHandler<GetEnrolledClassesCommand, ResponseDto<List<EnrolledClassDto>>>
    {
        private readonly IGenericRepository<StudentClass> repository;
        private readonly IMapper mapper;

        public GetEnrolledClassesHandler(IGenericRepository<StudentClass> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<EnrolledClassDto>>> Handle(GetEnrolledClassesCommand request, CancellationToken cancellationToken)
        {
            var studentId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var classes =await repository
                .GetAll()
                .Where(s => s.StudentId == studentId)
                .ProjectTo<EnrolledClassDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (classes.Count == 0)
                return ResponseDto<List<EnrolledClassDto>>.Error(ErrorCode.NotFound, "No enrolled classes found");

            return ResponseDto<List<EnrolledClassDto>>.Success(classes, "Enrolled class retrieved successfully");
        }
    }
}
