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

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.UpdateCourse
{
    class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;

        public UpdateCourseHandler(IGenericRepository<Course> repository , IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = repository.GetByID(request.CourseDto.Id);

                if (course == null)
                    return ResponseDto<bool>.Error(ErrorCode.NotFound, $"Course with id {request.CourseDto.Id} not found.");

                mapper.Map(request.CourseDto, course);
                repository.Update(course);
                await repository.SaveChangesAsync();

                return ResponseDto<bool>.Success(true, "Course information Updated successfully");
            }
            catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to update course {ex.Message}");
            }

        }
    }
}
