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
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Domain.Enums;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Attendance.Student;

namespace SchoolManagementSystem.Application.Students.Attendances.Queries.GetAttendance
{
    public class GetAttendanceHandler : IRequestHandler<GetAttendanceQuery, ResponseDto<List<GetStudentAttendanceDto>>>
    {
        private readonly IGenericRepository<Attendance> repository;
        private readonly IMapper mapper;

        public GetAttendanceHandler(IGenericRepository<Attendance> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<GetStudentAttendanceDto>>> Handle(GetAttendanceQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var attendances = await repository
                .GetAll()
                .Where(s => s.StudentId == studentId)
                .ProjectTo<GetStudentAttendanceDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (attendances.Count == 0)
                return ResponseDto<List<GetStudentAttendanceDto>>.Error(ErrorCode.NotFound, "No attendance records found");

            return ResponseDto<List<GetStudentAttendanceDto>>.Success(attendances, "Attendance records retrieved successfully");
        }
    }
}
