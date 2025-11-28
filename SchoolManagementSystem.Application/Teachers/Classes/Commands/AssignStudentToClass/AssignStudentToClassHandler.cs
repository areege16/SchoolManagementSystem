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

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.AssignStudentToClass
{
    class AssignStudentToClassHandler : IRequestHandler<AssignStudentToClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<StudentClass> studentClassRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Class> classRepository;

        public AssignStudentToClassHandler(IGenericRepository<StudentClass> studentClassRepository,
                                           IMapper mapper,
                                           IGenericRepository<Class> classRepository)
        {
            this.studentClassRepository = studentClassRepository;
            this.mapper = mapper;
            this.classRepository = classRepository;
        }
        public async Task<ResponseDto<bool>> Handle(AssignStudentToClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var classExists = classRepository.GetByID(request.AssignStudentToClassDto.ClassId);
                if(classExists==null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Class not found");

                var studentExistsInClass = studentClassRepository
                .GetFiltered(sc =>
                    sc.StudentId == request.AssignStudentToClassDto.StudentId &&
                    sc.ClassId == request.AssignStudentToClassDto.ClassId , tracked : true)
                .Any();
                if (studentExistsInClass)
                    return ResponseDto<bool>.Error(ErrorCode.ValidationFailed, "Student already assigned to this class");

                var studentClass = mapper.Map<StudentClass>(request.AssignStudentToClassDto);
                studentClassRepository.Add(studentClass);
                await studentClassRepository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Student Assigned to class successfully");
            }catch
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, "Failed Assign Student  to class");
            }
        }
    }
}
