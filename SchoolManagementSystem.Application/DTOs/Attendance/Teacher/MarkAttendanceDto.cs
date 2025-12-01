using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Attendance.Teacher
{
    public class MarkAttendanceDto
    {
        public int ClassId { set; get; }
        public string StudentId { set; get; }
        public string MarkedByTeacherId { get; set; }
        public DateTime Date { set; get; }
        public AttendanceStatus Status { set; get; }
    }
}
