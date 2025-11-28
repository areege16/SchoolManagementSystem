using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Class: BaseNamedEntity
    {
        public bool IsActive { set; get; } = true;
        public Semester Semester { set; get; }
        public DateOnly StartDate { set; get; }
        public DateOnly EndDate { set; get; }

        [ForeignKey("Course")]
        public int CourseId { set; get; }
        public Course Course { set; get; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<StudentClass> StudentClasses { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }

    }
}

