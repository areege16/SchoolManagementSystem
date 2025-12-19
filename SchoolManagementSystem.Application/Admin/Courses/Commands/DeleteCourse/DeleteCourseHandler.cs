using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;


namespace SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse
{
     class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> courseRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IMemoryCache memoryCache;

        public DeleteCourseHandler(IGenericRepository<Course> courseRepository,
                                   IGenericRepository<Class> classRepository,
                                   IMemoryCache memoryCache)
        {
            this.courseRepository = courseRepository;
            this.classRepository = classRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        { 
            try
            {        
                var course =await courseRepository
                  .GetFiltered(d => d.Id == request.Id,asTracking:true)
                  .Include(d => d.Classes)
                  .FirstOrDefaultAsync(cancellationToken);

                if (course == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Course with id {request.Id} not found");

                if (course.Classes != null && course.Classes.Count > 0)
                {
                    classRepository.RemoveRange(course.Classes);
                    await classRepository.SaveChangesAsync(cancellationToken);
                }

                courseRepository.Remove(request.Id);
                await courseRepository.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKeys.CoursesList);

                return ResponseDto<bool>.Success(true, $"Course with id {request.Id} deleted successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to delete Course {ex.Message}");
            }
        }
    }
}
