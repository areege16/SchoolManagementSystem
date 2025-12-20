using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Attendance : BaseEntity
    {
        public DateOnly Date { set; get; }
        public AttendanceStatus Status { set; get; }

        [ForeignKey("Class")]
        public int ClassId { set; get; }
        public Class Class { set; get; }

        [ForeignKey("Student")]
        public string StudentId { set; get; }
        public Student Student { set; get; }

        [ForeignKey("MarkedByTeacher")]
        public string MarkedByTeacherId { get; set; }
        public Teacher MarkedByTeacher { get; set; }
    }
}