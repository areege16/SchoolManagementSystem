using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Assignments.Commands.CreateAssignment
{
    public class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Assignment> repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateAssignmentHandler> logger;

        public CreateAssignmentHandler(IGenericRepository<Assignment> repository,
                                       IMapper mapper,
                                       ILogger<CreateAssignmentHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to create assignment with title: {AssignmentTitle}", request.TeacherId, request.CreateAssignmentDto.Title);

                var newAssignment = mapper.Map<Assignment>(request.CreateAssignmentDto);
                newAssignment.CreatedByTeacherId = request.TeacherId;
                newAssignment.CreatedDate = DateTime.UtcNow;
                repository.Add(newAssignment);
                await repository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Teacher {TeacherId} successfully created assignment with Id {AssignmentId} with title: {AssignmentTitle}", request.TeacherId, newAssignment.Id, newAssignment.Title);

                return ResponseDto<bool>.Success(true, "Assignment created successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Teacher {TeacherId} failed to create assignment. Assignment title: {AssignmentTitle}", request.TeacherId, request.CreateAssignmentDto.Title);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to create assignment");
            }
        }
    }
}