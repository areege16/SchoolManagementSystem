using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
    class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> departmentRepository;
        private readonly IGenericRepository<Course> courseRepository;

        public DeleteDepartmentHandler(IGenericRepository<Department> departmentRepository,IGenericRepository<Course> courseRepository)
        {
            this.departmentRepository = departmentRepository;
            this.courseRepository = courseRepository;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = departmentRepository.
                   GetAll()
                  .Where(d => d.Id == request.Id)
                  .Include(d=>d.Courses)
                  .FirstOrDefault();

                if(department==null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Department with id {request.Id} not found");

                if (department.Courses != null && department.Courses.Count > 0)
                {
                    courseRepository.RemoveRange(department.Courses);
                    await courseRepository.SaveChangesAsync();
                }

                departmentRepository.Remove(request.Id);
                await departmentRepository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, $"Department with id {request.Id} deleted successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to delete department{ex.Message}");
            }

        }
    }
}
