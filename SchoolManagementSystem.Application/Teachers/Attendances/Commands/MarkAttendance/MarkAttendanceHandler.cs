using AutoMapper;
using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Attendances.Commands.MarkAttendance
{
    public class MarkAttendanceHandler : IRequestHandler<MarkAttendanceCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Attendance> repository;
        private readonly IMapper mapper;
        public MarkAttendanceHandler(IGenericRepository<Attendance> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var attendance = mapper.Map<Attendance>(request.AttendanceDto);
                repository.Add(attendance);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Attendance marked successfully");
            }catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.OperationFailed, $"Failed to marked attendance {ex.InnerException}");
            }
        }
    }
}
