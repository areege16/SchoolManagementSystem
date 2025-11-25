using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public DeleteCourseHandler(IGenericRepository<Course> courseRepository,IGenericRepository<Class> classRepository)
        {
            this.courseRepository = courseRepository;
            this.classRepository = classRepository;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        { 
            try
            {        
                var course = courseRepository.
                   GetAll()
                  .Where(d => d.Id == request.Id)
                  .Include(d => d.Classes)
                  .FirstOrDefault();

                if (course == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Course with id {request.Id} not found");

                if (course.Classes != null && course.Classes.Count > 0)
                {
                    classRepository.RemoveRange(course.Classes);
                    await classRepository.SaveChangesAsync();
                }
                courseRepository.Remove(request.Id);
                await courseRepository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, $"Course with id {request.Id} deleted successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to delete Course {ex.Message}");
            }
        }
    }
}
