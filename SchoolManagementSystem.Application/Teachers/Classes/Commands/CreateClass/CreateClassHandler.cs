using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;


namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    class CreateClassHandler : IRequestHandler<CreateClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateClassHandler> logger;

        public CreateClassHandler(IGenericRepository<Class> repository,
                                  IMapper mapper,
                                  ILogger<CreateClassHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to create class with name: {ClassName}", request.TeacherId, request.ClassDto.Name);

                var newClass = mapper.Map<Class>(request.ClassDto);
                newClass.TeacherId = request.TeacherId;
                repository.Add(newClass);
                await repository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Teacher {TeacherId} successfully created class with Id {ClassId} with name: {ClassName}", request.TeacherId, newClass.Id, newClass.Name);

                return ResponseDto<bool>.Success(true, "Class created successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Teacher {TeacherId} failed to create class. Class name: {ClassName}", request.TeacherId, request.ClassDto.Name);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to create class ");
            }
        }
    }
}