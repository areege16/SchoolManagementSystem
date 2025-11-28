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

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.CreateClass
{
    class CreateClassHandler : IRequestHandler<CreateClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;

        public CreateClassHandler(IGenericRepository<Class> repository ,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newClass = mapper.Map<Class>(request.classDto);
                repository.Add(newClass);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Class Created successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to create class {ex.Message}");
            }

        }
    }
}
