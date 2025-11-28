using AutoMapper;
using SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment;
using SchoolManagementSystem.Application.DTOs.Class;
using SchoolManagementSystem.Application.DTOs.Course;
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
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Course, CourseMiniDto>().ReverseMap();
            CreateMap<UpdateDepartmentDto, Department>()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion

            #region Course
            CreateMap<CreateCourseDto, Course>().ReverseMap();
            CreateMap<Course,CourseDto>().ReverseMap();
            CreateMap<Class, ClassMiniDto>().ReverseMap();
            CreateMap<UpdateCourseDto, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion

            #region Class
            CreateMap<CreateClassDto, Class>().ReverseMap();
            CreateMap<Class , ClassDto>()
                .ForMember(dest => dest.StudentsInClass,
                  opt => opt.MapFrom(src => src.StudentClasses)); 
            CreateMap<StudentClass, StudentsInClassDto>().ReverseMap();
            CreateMap<UpdateClassDto, Class>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<AssignStudentToClassDto, StudentClass>().ReverseMap();           
            #endregion
        }
    }
}
