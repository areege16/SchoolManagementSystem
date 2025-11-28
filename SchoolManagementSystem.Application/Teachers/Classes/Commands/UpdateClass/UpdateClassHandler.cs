using AutoMapper;
using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.UpdateClass
{
    class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;

        public UpdateClassHandler(IGenericRepository<Class> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updateClass = repository.GetByID(request.ClassDto.Id);
                if (updateClass == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Class Not found");

                mapper.Map(request.ClassDto, updateClass);
                repository.Update(updateClass);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Class Updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to update Class {ex.Message}");
            }
          }      
       }
    }

