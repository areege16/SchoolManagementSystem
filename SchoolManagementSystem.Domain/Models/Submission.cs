using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Submission //TODO add BaseEntity
    {
        public int Id { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string? FileUrl { get; set; }
        public double? Grade { get; set; }
        public string? Remarks { get; set; }


        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }


        [ForeignKey("Student")]
        public string StudentId { set; get; }
        public Student Student { set; get; }


        [ForeignKey("GradedByTeacher")]
        public string GradedByTeacherId { get; set; }
        public Teacher GradedByTeacher { get; set; }
     
    }
}
