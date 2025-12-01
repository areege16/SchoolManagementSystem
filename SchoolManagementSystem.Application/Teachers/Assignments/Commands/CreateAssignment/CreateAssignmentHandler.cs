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

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment
{
    public class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly IMapper mapper;

        public CreateAssignmentHandler(IGenericRepository<Assignment> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newAssignment = mapper.Map<Assignment>(request.CreateAssignmentDto);
                repository.Add(newAssignment);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Assignment Created successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to create Assignment {ex.Message}");
            }
        }
    }
}
