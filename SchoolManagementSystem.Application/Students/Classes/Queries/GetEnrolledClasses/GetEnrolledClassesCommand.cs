using MediatR;
using SchoolManagementSystem.Application.DTOs;
using SchoolManagementSystem.Application.DTOs.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Students.Classes.Queries.GetEnrolledClasses
{
    public class GetEnrolledClassesCommand : IRequest<ResponseDto<List<EnrolledClassDto>>>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
