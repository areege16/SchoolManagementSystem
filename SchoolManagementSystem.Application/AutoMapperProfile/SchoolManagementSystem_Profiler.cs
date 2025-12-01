using AutoMapper;
using SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment;
using SchoolManagementSystem.Application.DTOs.Assignment.Student;
using SchoolManagementSystem.Application.DTOs.Assignment.Teacher;
using SchoolManagementSystem.Application.DTOs.Attendance.Student;
using SchoolManagementSystem.Application.DTOs.Attendance.Teacher;
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
            CreateMap<StudentClass, EnrolledClassDto>()
                 .ForMember(dest => dest.ClassName, opt =>opt.MapFrom(src=>src.Class.Name))
                 .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Class.Course.Name))
                 .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Class.Teacher.ApplicationUser.Name))
                 .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Class.Semester))
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Class.StartDate))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Class.EndDate));
            #endregion

            #region Attendance
            CreateMap<MarkAttendanceDto, Attendance>();
            CreateMap<Attendance, AttendanceHistoryDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.ApplicationUser.Name));

            CreateMap<Attendance, GetStudentAttendanceDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Class.Course.Name))
                .ForMember(dest => dest.MarkedByTeacherName, opt => opt.MapFrom(src => src.Class.Teacher.ApplicationUser.Name));           
            #endregion

            #region Assignment
            CreateMap<CreateAssignmentDto, Assignment>();
            CreateMap<Assignment, AssignmentDto>();
            CreateMap<GradeStudentSubmissionDto, Submission>();
            CreateMap<Assignment, GetStudentAssignmentDto>()
                .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Class.Course.Name))
                .ForMember(dest => dest.CreatedByTeacherName, opt => opt.MapFrom(src => src.Class.Teacher.ApplicationUser.Name));
            CreateMap<Submission,GetStudentGradeDto>()
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment.Title));

            #endregion
        }
    }
}
