using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass
{
    class AssignStudentToClassHandler : IRequestHandler<AssignStudentToClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AssignStudentToClassHandler> logger;

        public AssignStudentToClassHandler(IGenericRepository<StudentClass> studentClassRepository,
                                           IGenericRepository<Class> classRepository,
                                           IMapper mapper,
                                           ILogger<AssignStudentToClassHandler> logger)
        {
            this.studentClassRepository = studentClassRepository;
            this.classRepository = classRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<bool>> Handle(AssignStudentToClassCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AssignStudentToClassDto;
            var teacherId = request.TeacherId;
            try
            {
                logger.LogInformation("Teacher {TeacherId} attempting to assign Student {StudentId} to Class {ClassId}", teacherId, dto.StudentId, dto.ClassId);

                var classInfo = await classRepository
                      .GetAllAsNoTracking()
                      .Where(c => c.Id == dto.ClassId)
                      .Select(c => c.TeacherId)
                      .SingleOrDefaultAsync(cancellationToken);

                if (classInfo == null)
                {
                    logger.LogWarning("Assignment failed: Class {ClassId} not found.", dto.ClassId);
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Class not found");
                }
                if (classInfo != teacherId)
                {
                    logger.LogWarning("Teacher {TeacherId} is not authorized to assign students to Class {ClassId} (Owner: {OwnerTeacherId}).", teacherId, dto.ClassId, classInfo);
                    return ResponseDto<bool>.Error(ErrorCode.Unauthorized, "You are not authorized to assign students to this class.");
                }

                var studentExistsInClass = await studentClassRepository
                .GetFiltered(sc => sc.StudentId == dto.StudentId && sc.ClassId == dto.ClassId, asTracking: false)
                .AnyAsync(cancellationToken);

                if (studentExistsInClass)
                {
                    logger.LogWarning("Assignment rejected: Student {StudentId} is already assigned to Class {ClassId}.", dto.StudentId, dto.ClassId);
                    return ResponseDto<bool>.Error(ErrorCode.ValidationFailed, "Student already assigned to this class");
                }
                var studentClass = mapper.Map<StudentClass>(dto);
                studentClass.EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                studentClassRepository.Add(studentClass);

                await studentClassRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Student {StudentId} successfully assigned to Class {ClassId} by Teacher {TeacherId}", dto.StudentId, dto.ClassId, teacherId);

                return ResponseDto<bool>.Success(true, "Student Assigned to class successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to assign Student {StudentId} to Class {ClassId} by Teacher {TeacherId}. Error: {ErrorMessage}", dto.StudentId, dto.ClassId, teacherId, ex.Message);

                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed Assign Student to class");
            }
        }
    }
}