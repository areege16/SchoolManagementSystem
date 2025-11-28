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

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.DeleteClass
{
    public class DeleteClassHandler : IRequestHandler<DeleteClassCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Class> repository;

        public DeleteClassHandler(IGenericRepository<Class> repository)
        {
            this.repository = repository;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cls = repository.GetByID(request.Id);
                if(cls==null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, "Class not found ");

                cls.IsActive = false;
                repository.Update(cls);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Class deleted successfully");
            }catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to delete class {ex.Message}");
            }
        }
    }
}
