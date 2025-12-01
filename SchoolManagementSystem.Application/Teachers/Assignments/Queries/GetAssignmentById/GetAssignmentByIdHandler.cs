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
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Queries.GetAssignmentById
{
    class GetAssignmentByIdHandler : IRequestHandler<GetAssignmentByIdQuery, ResponseDto<AssignmentDto>>
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly IMapper mapper;

        public GetAssignmentByIdHandler(IGenericRepository<Assignment> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<AssignmentDto>> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await repository.GetAll()
                           .Where(d => d.Id == request.Id)
                           .ProjectTo<AssignmentDto>(mapper.ConfigurationProvider)
                           .FirstOrDefaultAsync(cancellationToken);

            if (assignment == null)
                return ResponseDto<AssignmentDto>.Error(ErrorCode.NotFound, $"No Assignment with id {request.Id} found");

            return ResponseDto<AssignmentDto>.Success(assignment, $"Assignment with id {request.Id} retrieved successfully");
        }
    }
}
