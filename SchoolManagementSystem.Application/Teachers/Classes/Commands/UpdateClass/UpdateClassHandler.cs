using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.UpdateClass
{
    class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateClassHandler> logger;

        public UpdateClassHandler(IGenericRepository<Class> repository,
                                  IMapper mapper,
                                  ILogger<UpdateClassHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ClassDto;
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to update class with id: {ClassId}", request.TeacherId, dto.Id);

                var classTeacherId = await repository
                    .GetAllAsNoTracking()
                    .Where(c => c.Id == dto.Id)
                    .Select(c => c.TeacherId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (classTeacherId != teacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} is not authorized to update Class {ClassId} (Owner: {OwnerTeacherId}).", teacherId, dto.Id, classTeacherId);
                    return ResponseDto<bool>.Error(ErrorCode.Unauthorized, "You are not authorized to update this class.");
                }

                var updateClass = await repository.FindByIdAsync(dto.Id, cancellationToken);

                if (updateClass == null)
                {
                    logger.LogWarning("Teacher {TeacherId} tried to update non-existent class {ClassId}", request.TeacherId, dto.Id);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Class Not found");
                }

                mapper.Map(dto, updateClass);
                updateClass.UpdatedDate = DateTime.UtcNow;

                await repository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Teacher {TeacherId} successfully updated class {ClassId}", request.TeacherId, dto.Id);

                return ResponseDto<bool>.Success(true, "Class Updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Teacher {TeacherId} failed to update class {ClassId}", request.TeacherId, dto.Id);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed to update Class");
            }
        }
    }
}