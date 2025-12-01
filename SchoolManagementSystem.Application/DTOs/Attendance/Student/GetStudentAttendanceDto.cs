using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Attendance.Student
{
   public class GetStudentAttendanceDto
    {
        public int ClassId { set; get; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public DateTime Date { set; get; }
        public AttendanceStatus Status { set; get; }
        public string MarkedByTeacherName { get; set; }
    }
}
