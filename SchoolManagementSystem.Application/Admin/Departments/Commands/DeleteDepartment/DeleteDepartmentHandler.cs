using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;

namespace SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment
{
    class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Department> departmentRepository;
        private readonly IGenericRepository<Course> courseRepository;
        private readonly IMemoryCache memoryCache;

        public DeleteDepartmentHandler(IGenericRepository<Department> departmentRepository,
                                       IGenericRepository<Course> courseRepository,
                                       IMemoryCache memoryCache)
        {
            this.departmentRepository = departmentRepository;
            this.courseRepository = courseRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await departmentRepository
                  .GetFiltered(d => d.Id == request.Id, asTracking: true)
                  .Include(d => d.Courses)
                  .FirstOrDefaultAsync(cancellationToken);

                if (department == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Department with id {request.Id} not found");

                if (department.Courses != null && department.Courses.Count > 0)
                {
                    courseRepository.RemoveRange(department.Courses);
                    await courseRepository.SaveChangesAsync(cancellationToken);
                }

                departmentRepository.Remove(request.Id);
                await departmentRepository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.DepartmentsList);

                return ResponseDto<bool>.Success(true, $"Department with id {request.Id} deleted successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to delete department{ex.Message}");
            }
        }
    }
}