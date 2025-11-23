using AutoMapper;
using SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.AutoMapperProfile
{
    public class SchoolManagementSystem_Profiler :Profile
    {
        public SchoolManagementSystem_Profiler()
        {

            #region Department
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
            CreateMap<UpdateDepartmentDto, Department>().ReverseMap();
            CreateMap<DepartmentDto, Department>().ReverseMap();
            CreateMap<Course, CourseMiniDto>().ReverseMap(); 
            #endregion


        }
    }
}
