using MediatR;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Enums;
using Azure.Core;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Queries.GetAttendanceHistory
{
    public class GetAttendanceHistoryHandler : IRequestHandler<GetAttendanceHistoryCommand, ResponseDto<List<AttendanceHistoryDto>>>//TODO : Change response to be grouped by Date instead of flat list.
    {
        private readonly IGenericRepository<Attendance> repository;
        private readonly IMapper mapper;

        public GetAttendanceHistoryHandler(IGenericRepository<Attendance> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<AttendanceHistoryDto>>> Handle(GetAttendanceHistoryCommand request, CancellationToken cancellationToken)
        {
            var attendanceHistory =await repository
                .GetFiltered(i => i.ClassId == request.ClassId,tracked:false)              
                .ProjectTo<AttendanceHistoryDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (attendanceHistory == null)
                return ResponseDto<List<AttendanceHistoryDto>>.Error(ErrorCode.NotFound, $"No attendance history with id {request.ClassId} found");

            return ResponseDto<List<AttendanceHistoryDto>>.Success(attendanceHistory, $"Attendance history with id {request.ClassId} retrieved successfully");
        }
    }
}
