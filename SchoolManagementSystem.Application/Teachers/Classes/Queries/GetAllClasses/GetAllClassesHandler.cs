using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
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
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses
{
    class GetAllClassesHandler : IRequestHandler<GetAllClassesCommand, ResponseDto<List<ClassDto>>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;

        public GetAllClassesHandler(IGenericRepository<Class> repository , IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<ResponseDto<List<ClassDto>>> Handle(GetAllClassesCommand request, CancellationToken cancellationToken)
        {
            var classes =await repository
                .GetAll()
                .ProjectTo<ClassDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (classes.Count == 0)
                return ResponseDto<List<ClassDto>>.Error(ErrorCode.NotFound, "No Class found");

            return ResponseDto<List<ClassDto>>.Success(classes, "Classes retrieved successfully");
        }
    }
}
