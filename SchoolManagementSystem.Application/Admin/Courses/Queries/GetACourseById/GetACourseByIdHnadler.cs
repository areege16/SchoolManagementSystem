using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Admin.Courses.Queries.GetACourseById
{
    class GetACourseByIdHnadler : IRequestHandler<GetACourseByIdQuery, ResponseDto<CourseDto>>
    {
        private readonly IGenericRepository<Course> repository;
        private readonly IMapper mapper;

        public GetACourseByIdHnadler(IGenericRepository<Course> repository , IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<CourseDto>> Handle(GetACourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await repository
                .GetAll()
                .Where(c => c.Id == request.Id)
                .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (course == null)
                return ResponseDto<CourseDto>.Error(ErrorCode.NotFound, $"No course with id {request.Id} found");

            return ResponseDto<CourseDto>.Success(course, $"Course with id {request.Id} retrieved successfully");
        }
    }
}
