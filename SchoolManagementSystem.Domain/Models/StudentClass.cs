using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class StudentClass
    {
        public DateOnly EnrollmentDate { set; get; }

        [ForeignKey("Student")]
        public string StudentId { set; get; }
        public Student Student { set; get; }

        [ForeignKey("Class")]
        public int ClassId { set; get; }
        public Class Class { set; get; }
    }
}