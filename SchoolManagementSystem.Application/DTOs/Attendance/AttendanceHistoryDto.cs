using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Attendance
{
    public class AttendanceHistoryDto
    {
        public int Id { set; get; }
        public int ClassId { set; get; }
        public string StudentId { set; get; }
        public string StudentName { set; get; }
        public string MarkedByTeacherId { get; set; }
        public AttendanceStatus Status { set; get; }
        public DateTime Date { set; get; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}

