using MediatR;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetAllCourses
{
    class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, ResponseDto<List<CourseDto>>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;

        public GetAllCoursesHandler(IGenericRepository<Course> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var Courses = await repository
                .GetAll()
                .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if(Courses.Count==0)
                return ResponseDto<List<CourseDto>>.Error(ErrorCode.NotFound, "No courses found");

            return ResponseDto<List<CourseDto>>.Success(Courses, "Courses retrieved successfully");
        }
    }
}
