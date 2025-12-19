using MediatR;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Domain.Models;
using AutoMapper;
using SchoolManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper.QueryableExtensions;

namespace SchoolManagementSystem.Application.Teachers.Classes.Queries.GetAllClasses
{
    class GetAllClassesHandler : IRequestHandler<GetAllClassesCommand, ResponseDto<List<ClassDto>>>
    {
        private readonly IGenericRepository<Class> repository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAllClassesHandler> logger;

        public GetAllClassesHandler(IGenericRepository<Class> repository,
                                    IMapper mapper,
                                    ILogger<GetAllClassesHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<ResponseDto<List<ClassDto>>> Handle(GetAllClassesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Retrieving all classes for teacher.");

                var classes = await repository
                .GetAllAsNoTracking()
                .Where(c => c.TeacherId == request.TeacherId)
                .ProjectTo<ClassDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                if (classes.Count == 0)
                {
                    logger.LogWarning("No classes found for Teacher {TeacherId}.", request.TeacherId);
                    return ResponseDto<List<ClassDto>>.Error(ErrorCode.NotFound, "No Class found");
                }

                logger.LogInformation("Successfully retrieved {ClassCount} classes.", classes.Count);

                return ResponseDto<List<ClassDto>>.Success(classes, "Classes retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving classes.");
                return ResponseDto<List<ClassDto>>.Error(ErrorCode.DatabaseError, "Failed to retrieve classes.");
            }
        }
    }
}