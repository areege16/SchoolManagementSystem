using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class StudentClass //TODO add BaseEntity
    {
        public int Id { set; get; }
        public DateTime EnrollmentDate { set; get; }

        [ForeignKey("Student")]
        public string StudentId { set; get; }
        public Student Student { set; get; }

        [ForeignKey("Class")]
        public int ClassId { set; get; }
        public Class Class { set; get; }


    }
}
