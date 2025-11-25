using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourses;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.CreateCourse
{
    class CreateCourseHandler : IRequestHandler<CreateCourseCommand, ResponseDto<bool>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;

        public CreateCourseHandler(IGenericRepository<Course> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<bool>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = mapper.Map<Course>(request.CourseDto);
                repository.Add(course);
                await repository.SaveChangesAsync();
                return ResponseDto<bool>.Success(true, "Course Added successfully");

            }catch(Exception ex)
            {
                return ResponseDto<bool>.Error(ErrorCode.DatabaseError, $"Failed to add Course {ex.Message}");
            }
        }
    }
}
