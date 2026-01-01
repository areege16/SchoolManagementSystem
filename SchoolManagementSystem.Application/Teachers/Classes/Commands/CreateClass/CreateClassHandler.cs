using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.Common.Responses;
using SchoolManagementSystem.Application.DTOs.Notification;
using SchoolManagementSystem.Application.Services.NotificationStreamService;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    class CreateClassHandler : IRequestHandler<CreateClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IGenericRepository<Student> studentRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateClassHandler> logger;
        private readonly INotificationStreamService notificationStreamService;

        public CreateClassHandler(IGenericRepository<Class> repository,
                                  IGenericRepository<Student> studentRepository,
                                  IMapper mapper,
                                  ILogger<CreateClassHandler> logger,
                                  INotificationStreamService notificationStreamService)
        {
            this.repository = repository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.notificationStreamService = notificationStreamService;
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

                var students = await studentRepository
                    .GetAllAsNoTracking()
                    .ToListAsync(cancellationToken);

                var notificationDto = new NotificationDto
                {
                    Title = "New Class Created",
                    Message = $"A new class ' {newClass.Name} ' has been created by your teacher.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var notificationTasks = students.Select(student => notificationStreamService.NotifyUser(student.Id, notificationDto));
                await Task.WhenAll(notificationTasks);

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